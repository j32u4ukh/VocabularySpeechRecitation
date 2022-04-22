﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Text To Speech Controller
    /// 
    ///(*) Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///(*) When using value save (saveSetting), it is better to give a specific save name (saveKey) individually.
    ///
    /// (Locale)
    ///･Format : "language_country" (e.g. "en", "en_US", "ja", "ja_JP")
    ///･language : ISO 639 alpha-2 or alpha-3 language code
    ///･country(region) : ISO 639 alpha-2 or alpha-3 language code
    /// https://developer.android.com/reference/java/util/Locale
    /// (Locale list)
    /// http://fantom1x.blog130.fc2.com/blog-entry-295.html
    /// </summary>
    public class TextToSpeechController : MonoBehaviour
    {
        // status message
        [Serializable] public class StopHandler : UnityEvent<string> { }

        // status message
        [Serializable] public class StatusHandler : UnityEvent<string> { }

        // Speed after change
        [Serializable] public class SpeedChangedHandler : UnityEvent<float> { }

        // Pitch after change
        [Serializable] public class PitchChangedHandler : UnityEvent<float> { }

        // Inspector Settings
        // Locale (e.g. "en", "en_US", "ja", "ja_JP") / empty = system default
        [SerializeField] private string locale = "";

        // Speed and pitch increase/decrease amount. To increase the decimal places, it need to increase pow10 (static variable).
        const float STEP_MIN = 0.01f;

        #region Inspector Settings
        // Speech speed (1.0 is recommended on startup)
        [SerializeField, Range(0.5f, 2.0f)] private float speed = 1;

        // Speech speed increase/decrease amount
        [SerializeField, Range(STEP_MIN, 2.0f)] private float speedStep = 0.05f;

        // Voice pitch (1.0 is recommended on startup)
        [SerializeField, Range(0.5f, 2.0f)] private float pitch = 1;

        // Voice pitch increase/decrease amount
        [SerializeField, Range(STEP_MIN, 2.0f)] private float pitchStep = 0.05f;
        #endregion

        #region Save PlayerPrefs Settings
        // Whether to save the settings (Also local value is always overwritten).
        public bool saveSetting = false;

        // When specifying the PlayerPrefs key for settings.
        [SerializeField] private string saveKey = "";
        #endregion

        #region Callbacks
        // Callback when speech started
        public UnityEvent OnStart;

        // Callback when speech finished
        public UnityEvent OnDone;
        #endregion

        // Callback when interrupted
        public StopHandler OnStop;

        // Callback when initialize and errors
        public StatusHandler OnStatus;

        // Callback when speed changed
        public SpeedChangedHandler OnSpeedChanged;

        // Callback when pitch changed
        public PitchChangedHandler OnPitchChanged;

        static int[] pow10 = { 1, 10, 100 };  //10^k : Fit to decimal part : (0,)1~2 [0.01~2.0]

        #region PlayerPrefs Section

        //Defalut PlayerPrefs Key (It is used only when saveKey is empty)
        const string SETTING_PREF = "_setting";
        const string SPEED_KEY = "speed";
        const string PITCH_KEY = "pitch";
        const string SPEED_STEP_KEY = "speedStep";
        const string PITCH_STEP_KEY = "pitchStep";
        const string LOCALE_KEY = "locale";

        private Param setting = new Param();    //Compatibale Dictionary <string, string>

        //Saved key in PlayerPrefs (Default key is "gameObject.name + '_setting'")
        public string SaveKey {
            get { return string.IsNullOrEmpty(saveKey) ? gameObject.name + SETTING_PREF : saveKey; }
        }

        //Returns true if the saved value exists.
        public bool HasPrefs {
            get { return PlayerPrefs.HasKey(SaveKey); }
        }

        #endregion

        #region Properties and Local values Section

        // Speech speed (Because TTS shares one, it is static).
        private static float sSpeed;    

        // Voice pitch (Because TTS shares one, it is static).
        private static float sPitch;    

        // 數值設定範圍: 0.5f ~ 2.0f
        // The currently (default) speech speed.
        // If saveValue is true, it will be automatically saved.
        public float Speed {
            get { return sSpeed; }
            set {
                // For check the change of value.
                float oldSpeed = sSpeed;  
                
                SetSpeed(value);

                if (oldSpeed != sSpeed)
                {
                    if (OnSpeedChanged != null) 
                    {
                        OnSpeedChanged.Invoke(sSpeed);
                    }

                    if (saveSetting)
                    {
                        SavePrefs();
                    }
                }
            }
        }

        //The currently (default) voice pitch.
        //･If saveValue is true, it will be automatically saved.
        public float Pitch {
            get { return sPitch; }
            set {
                float oldPitch = sPitch;    //For check the change of value.
                SetPitch(value);
                if (oldPitch != sPitch)
                {
                    if (OnPitchChanged != null)
                        OnPitchChanged.Invoke(sPitch);

                    if (saveSetting)
                        SavePrefs();
                }
            }
        }

        //Increase/decrease amount of speed
        public float SpeedStep {
            get { return speedStep; }
            set {
                speedStep = Mathf.Clamp(value, STEP_MIN, 2.0f);
                if (saveSetting)
                    SavePrefs();
            }
        }

        //Increase/decrease amount of pitch
        public float PitchStep {
            get { return pitchStep; }
            set {
                pitchStep = Mathf.Clamp(value, STEP_MIN, 2.0f);
                if (saveSetting)
                    SavePrefs();
            }
        }

        //Initialization state
        private int initialized = -1;            //1:success, 0:fail, -1:not yet
        private string initializeStatus = "";    //initialize status message

        //Initialization succeeded (Always false before initialization).
        public bool IsInitializeSuccess 
        {
            get { 
                return initialized == 1; 
            }
        }

        //Status message when initialized
        public string InitializeStatus 
        {
            get { 
                return initializeStatus; 
            }
        }

        //Last of received status message
        public string StatusMessage {
            get; private set;
        }

        // Change locale (empty is system default)
        public string Locale {
            get { return locale; }
            set {
                if (locale == value)
                {
                    OnStatus.Invoke($"Locale is not changed.");
                }
                else
                {
                    // OnStop 有可能不被觸發，因為馬上就呼叫 Release 了
                    // Note: "OnStop" may not occur because it will be released soon.
                    // すぐに「Release()」されるため、「OnStop」は発生しないこともあるので注意。
                    StopSpeech();
                    Release();

                    locale = (value == AndroidLocale.Default) ? "" : value;

                    InitializeTextToSpeech();

                    // Restore, because it was "Release()"
                    SetSpeed(sSpeed);

                    // Restore, because it was "Release()"
                    SetPitch(sPitch);

                    if (saveSetting)
                    {
                        SavePrefs();
                    }
                }
            }
        }

#endregion

        // Use this for initialization
        private void Awake()
        {
            //move to static field (Things like singleton)
            sSpeed = speed;     //from inspector setting
            sPitch = pitch;     //from inspector setting

            // If the locale is changed, 'InitializeTextToSpeech()' is called.
            if (saveSetting)
            {
                LoadPrefs();
            }

            // not yet
            if (initialized == -1) 
            {
                StopSpeech();
                Release();

                // 初始化
                InitializeTextToSpeech();

                // Restore, because it was "Release()"
                SetSpeed(sSpeed);

                // Restore, because it was "Release()"
                SetPitch(sPitch);

                if (saveSetting)
                {
                    SavePrefs();
                }
            }
        }

        // Initialize Text To Speech
        // Note: The result is returned to the status callback.
        public void InitializeTextToSpeech()
        {
            ResetInitializeStatus();

#if UNITY_EDITOR
            Debug.Log("InitTextToSpeech");

#elif UNITY_ANDROID
            // Get a initialization status
            if (string.IsNullOrEmpty(locale))
            {
                AndroidPlugin.InitTextToSpeech(gameObject.name, "ReceiveStatus");
            }

            // Get a initialization status
            else 
            {
                AndroidPlugin.InitTextToSpeech(locale, gameObject.name, "ReceiveStatus");
            }
#endif
        }

        /// <summary>
        /// 重置狀態
        /// Return to the status status that has not been initialized yet.
        /// </summary>
        private void ResetInitializeStatus()
        {
            initialized = -1;
            initializeStatus = "";
        }

        // Start Text To Speech
        public void StartSpeech(string text)
        {
            if (string.IsNullOrEmpty(text) || !IsInitializeSuccess)
            {
                return;
            }
                
#if UNITY_EDITOR
            Debug.Log("TextToSpeechController.StartSpeech : text = " + text);

#elif UNITY_ANDROID
            AndroidPlugin.StartTextToSpeech(
                text, 
                gameObject.name,
                "ReceiveStatus", 
                "ReceiveStart",
                "ReceiveDone",
                "ReceiveStop");
#endif
        }

        //Speech started state
        private void ReceiveStart(string message)
        {
            if (OnStart != null)
            {
                OnStart.Invoke();
            }
        }

        //Speech finished state
        private void ReceiveDone(string message)
        {
            if (OnDone != null)
            {
                OnDone.Invoke();
            }
        }

        //Speech interrupted state
        private void ReceiveStop(string message)
        {
            if (OnStop != null)
            {
                OnStop.Invoke(message);
            }
        }

        // Receive status including initialization and errors.
        private void ReceiveStatus(string message)
        {
            StatusMessage = message;

            // At first time (As long as it is the same language)
            if (initialized == -1)  
            {
                initialized = message.StartsWith("SUCCESS_INIT") ? 1 : 0;
                initializeStatus = message;
            }

            if (OnStatus != null)
            {
                OnStatus.Invoke(message);
            }
        }

        // Interrupt speech
        public void StopSpeech()
        {
#if UNITY_EDITOR
            Debug.Log("TextToSpeechController.StopSpeech called");
#elif UNITY_ANDROID
            // 'OnStop' event occurs.
            AndroidPlugin.StopTextToSpeech();   
#endif
        }

        // Release TTS
        public void Release()
        {
#if UNITY_EDITOR
            Debug.Log("TextToSpeechController.Release called");
#elif UNITY_ANDROID
            AndroidPlugin.ReleaseTextToSpeech();
#endif
        }

        #region Speed
        // Speed control
        private void SetSpeed(float speed)
        {
#if UNITY_EDITOR
            sSpeed = Mathf.Clamp(speed, 0.5f, 2.0f);
#elif UNITY_ANDROID
            // returns: 0.5f ~ 2.0f
            sSpeed = AndroidPlugin.SetTextToSpeechSpeed(speed); 
#endif
        }

        //Add to the current speed.
        public void SpeedAdd(float add)
        {
            Speed += add;
        }

        //Reset the speed to 1.0.
        public void SpeedReset()
        {
            Speed = 1;
        }

        //Increase speed (Increase/decrease for each speedStep with 1.0 as the origin).
        public void SpeedUp()
        {
            Speed = MultipleFloor(sSpeed + speedStep, speedStep);
        }

        //Decrease speed (Increase/decrease for each speedStep with 1.0 as the origin).
        public void SpeedDown()
        {
            Speed = MultipleCeil(sSpeed - speedStep, speedStep);
        }
        #endregion

        #region Pitch
        // Pitch control
        private void SetPitch(float pitch)
        {
#if UNITY_EDITOR
            sPitch = Mathf.Clamp(pitch, 0.5f, 2.0f);
#elif UNITY_ANDROID
            // returns: 0.5f ~ 2.0f
            sPitch = AndroidPlugin.SetTextToSpeechPitch(pitch); 
#endif
        }

        //Add to the current pitch.
        public void PitchAdd(float add)
        {
            Pitch += add;
        }

        // Reset the pitch to 1.0.
        public void PitchReset()
        {
            Pitch = 1;
        }

        // Increase pitch (Increase/decrease for each pitchStep with 1.0 as the origin).
        public void PitchUp()
        {
            Pitch = MultipleFloor(sPitch + pitchStep, pitchStep);
        }

        //Decrease pitch (Increase/decrease for each pitchStep with 1.0 as the origin).
        public void PitchDown()
        {
            Pitch = MultipleCeil(sPitch - pitchStep, pitchStep);
        } 
        #endregion

        #region PlayerPrefs Section
        // Load local values manually.
        public void LoadPrefs()
        {
            setting = Param.GetPlayerPrefs(SaveKey, setting);
            speedStep = Mathf.Clamp(setting.GetFloat(SPEED_STEP_KEY, speedStep), STEP_MIN, 2.0f);
            pitchStep = Mathf.Clamp(setting.GetFloat(PITCH_STEP_KEY, pitchStep), STEP_MIN, 2.0f);

            float oldSpeed = sSpeed;    //For check the change of value.
            SetSpeed(setting.GetFloat(SPEED_KEY, sSpeed));

            if (oldSpeed != sSpeed && OnSpeedChanged != null)
            {
                OnSpeedChanged.Invoke(sSpeed);
            }

            // For check the change of value.
            float oldPitch = sPitch;

            SetPitch(setting.GetFloat(PITCH_KEY, sPitch));

            if (oldPitch != sPitch && OnPitchChanged != null)
            {
                OnPitchChanged.Invoke(sPitch);
            }

            Locale = setting.GetString(LOCALE_KEY, Locale);
        }

        // Save local values manually.
        public void SavePrefs()
        {
            setting.Set(SPEED_KEY, sSpeed);
            setting.Set(PITCH_KEY, sPitch);
            setting.Set(SPEED_STEP_KEY, speedStep);
            setting.Set(PITCH_STEP_KEY, pitchStep);
            setting.Set(LOCALE_KEY, Locale);
            Param.SetPlayerPrefs(SaveKey, setting);
            PlayerPrefs.Save();
        }

        // Delete PlayerPrefs key.
        // Note: Local values are not initialized at this time.
        public void DeletePrefs()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        } 
        #endregion

        #region Other method Section

        //Calculate smaller multiple (round up by multiple).
        //より小さい倍数を求める（倍数で切り捨てられるような値）
        float MultipleFloor(float value, float multiple)
        {
            int k = multiple.ToString().Length - 2;     //Number of digits after the decimal point. (e.g.) "0.05" -> "0." (Integer part = 1+'.'=2) + "05" (Decimal part = 4-2=2) 
            int p = pow10[k];                           //10^k
            int v = Mathf.RoundToInt((value - 1) * p);  //The origin is set to 1 (-1), and multiplied by 10^k.
            int m = Mathf.RoundToInt(multiple * p);     //To set it to an integer, because it causes errors as it is a float.
            float f = Mathf.Floor((float)v / m) * m;    //If calculate it as float, get an error here. (e.g.) Mathf.Floor(1.05f / 0.05f)=20 -> Mathf.Floor(105f / 5)=21
            return f / p + 1;                           //In order to set the origin to 1, restore it (+1).
        }

        //Calculate larger multiple (round down by multiple)
        //より大きい倍数を求める（倍数で繰り上がるような値）
        float MultipleCeil(float value, float multiple)
        {
            int k = multiple.ToString().Length - 2; 
            int p = pow10[k];
            int v = Mathf.RoundToInt((value - 1) * p);
            int m = Mathf.RoundToInt(multiple * p);
            float f = Mathf.Ceil((float)v / m) * m;
            return f / p + 1;
        }

        #endregion

    }
}

