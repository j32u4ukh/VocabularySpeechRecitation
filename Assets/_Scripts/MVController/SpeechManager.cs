using SpeechLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace VTS
{
    // TODO: 中斷說話的函式 STOP()
    public class SpeechManager : MonoBehaviour, ISpeech
    {
        private static SpeechManager instance = null;
        private State state = State.Idle;

        #region 時間相關設定
        float wait_time = 0.01f;
        float interval_time = 1.0f;
        float waiting_limit_time = 20.0f;
        WaitForSeconds wait;
        WaitForSeconds interval; 
        #endregion

        #region SpeechLib
        SpVoice voice;
        ISpeechObjectTokens tokens;
        event Action<string> onStatus;
        event Action onStart;
        event Action onDone;
        event Action<string> onStop;
        #endregion

        #region FantomLib
        [SerializeField] FantomLib.TextToSpeechController controller;
        #endregion

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            wait = new WaitForSeconds(wait_time);
            interval = new WaitForSeconds(interval_time);

            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                voice = new SpVoice();
                tokens = voice.GetVoices(string.Empty, string.Empty);
                controller.gameObject.SetActive(false);

                onStatus += onStatusListener;
                onStart += onStartListener;
                onDone += onDoneListener;
                onStop += onStopListener;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                controller.gameObject.SetActive(true);

                controller.OnStatus.AddListener(onStatusListener);
                controller.OnStart.AddListener(onStartListener);
                controller.OnDone.AddListener(onDoneListener);
                controller.OnStop.AddListener(onStopListener);

                // 主動對語言進行設置，避免無法在第一次設置語言後立即念誦的問題
                StartCoroutine(preSetLanguage());
            }
        }

        public static SpeechManager getInstance()
        {
            return instance;
        }

        #region 實作 ISpeech
        public void onStatusListener(string message)
        {
            setState(state: State.Status);
        }

        public void onStartListener()
        {
            setState(state: State.Start);
        }

        public void onDoneListener()
        {
            setState(state: State.Done);
        }

        public void onStopListener(string message)
        {
            setState(state: State.Stop);
        }
        #endregion

        IEnumerator preSetLanguage()
        {
            setLanguage(language: SystemLanguage.English);
            yield return StartCoroutine(waitForRelease());
        }

        /// <summary>
        /// SpVoice
        /// 0: Microsoft Hanhan Desktop - Chinese (Taiwan)
        /// 1: Microsoft Zira Desktop - English (United States)
        /// 2: Microsoft Haruka Desktop - Japanese
        /// 3: Microsoft David Desktop - English (United States)
        /// FantomLib
        /// 利用 Utils.getLanguageCode 取得相對應的語言代碼，若不在管理的列表中，預設使用繁中
        /// </summary>
        /// <param name="language"></param>
        public void setLanguage(SystemLanguage language)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string lang = language.ToString();

                // 會因不同電腦有安裝的語言套件而有所不同，這裡的索引值對應的語言是在我的電腦才成立的，
                // 但本來也就不是要提供電腦版，只是方便測試才讓電腦版也可以發聲
                switch (language)
                {
                    case SystemLanguage.English:
                        voice.Voice = tokens.Item(1);
                        break;
                    case SystemLanguage.Japanese:
                        voice.Voice = tokens.Item(2);
                        break;
                    case SystemLanguage.ChineseTraditional:
                    case SystemLanguage.Chinese:
                    default:
                        voice.Voice = tokens.Item(0);
                        lang = SystemLanguage.Chinese.ToString();
                        break;
                }

                onStatus?.Invoke($"Set language to {lang}");
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                string language_code = Utils.getLanguageCode(language: language);

                if (language_code != null)
                {
                    controller.Locale = language_code;
                }
                else
                {
                    controller.Locale = Utils.getLanguageCode(language: SystemLanguage.ChineseTraditional);
                }
            }
        }

        public void speak(string content)
        {
            Utils.log($"content: {content}");

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                onStart?.Invoke();
                voice.Speak(content);
                onDone?.Invoke();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                setState(state: State.Start);
                controller.StartSpeech(content);
            }
        }

        /// <summary>
        /// SpVoice 的語速設定範圍 -10(最慢) ~ 10(最快)
        /// FantomLib 的語速設定範圍 0.5f(最慢) ~ 2.0f(最快)
        /// </summary>
        /// <param name="speed"></param>
        public void setSpeed(float speed)
        {
            setState(state: State.Idle);

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                // https://docs.microsoft.com/zh-tw/dotnet/api/system.speech.synthesis.speechsynthesizer.rate?view=netframework-4.8
                voice.Rate = Mathf.Clamp((int)Math.Round(speed) - 1, -10, 10);
                onStatus?.Invoke($"Set speed to {voice.Rate}");
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                controller.Speed = speed;
            }
        }

        public void startReciteContent(string vocabulary, string description, SystemLanguage target, SystemLanguage describe, ReciteMode[] modes, Action callback = null)
        {
            StartCoroutine(reciteContent(vocabulary: vocabulary, 
                                         description: description, 
                                         target: target, 
                                         describe: describe, 
                                         modes: modes,
                                         callback: callback));
        }

        IEnumerator reciteContent(string vocabulary, string description, SystemLanguage target, SystemLanguage describe, ReciteMode[] modes, Action callback = null)
        {
            yield return StartCoroutine(waitForIdle());

            SystemLanguage language = SystemLanguage.Chinese;
            string content = string.Empty;

            foreach (ReciteMode mode in modes)
            {
                switch (mode)
                {
                    case ReciteMode.Word:
                        language = target;
                        content = vocabulary;
                        break;

                    case ReciteMode.Description:
                        language = describe;
                        content = description;
                        break;

                    case ReciteMode.Spelling:
                        language = target;
                        content = getSpelling(content: vocabulary, language: target);
                        break;

                    case ReciteMode.Interval:
                        yield return interval;
                        continue;
                }

                setLanguage(language: language);
                yield return StartCoroutine(waitForRelease());

                speak(content: content);
                yield return StartCoroutine(waitForRelease());
            }

            callback?.Invoke();
        }

        /// <summary>
        /// 等待 state 被重置回 State.Idle，才能開始執行切換語言或唸誦的指令。
        /// </summary>
        /// <returns></returns>
        IEnumerator waitForIdle()
        {
            float waiting_time = 0f;

            while ((state != State.Idle) && (waiting_time < waiting_limit_time))
            {
                waiting_time += wait_time;
                yield return wait;
            }

            if(waiting_time >= waiting_limit_time)
            {
                Utils.warn($"等待時間超時, waiting_time: {waiting_time}");
                setState(state: State.Idle);
            }
        }

        /// <summary>
        /// 結束等待後，state 一定要重置回 State.Idle，才能確保狀態變化的等待，
        /// 例如 State.Start 變成 State.Done 表示說話結束。若保持在 State.Done，來不及等待就結束了
        /// </summary>
        /// <returns></returns>
        IEnumerator waitForRelease()
        {
            float waiting_time = 0f;

            while ((state != State.Done) && (state != State.Stop) && (state != State.Status) && (waiting_time < waiting_limit_time))
            {
                waiting_time += wait_time;
                yield return wait;
            }

            setState(state: State.Idle);
        }

        string getSpelling(string content, SystemLanguage language = SystemLanguage.ChineseTraditional)
        {
            string[] split = content.ToCharArray().Select(c => c.ToString()).ToArray();
            string spelling;

            switch (language)
            {
                case SystemLanguage.English:
                    spelling = string.Join(",", split);
                    break;
                case SystemLanguage.Japanese:
                case SystemLanguage.ChineseTraditional:
                case SystemLanguage.Chinese:
                default:
                    spelling = string.Join("、", split);
                    break;
            }

            return spelling;
        }

        /// <summary>
        /// 設置等待時間，並更新間隔的秒數物件
        /// </summary>
        /// <param name="value"></param>
        public void setWaitTime(float value)
        {
            wait_time = value;
            wait = new WaitForSeconds(value);
        }

        /// <summary>
        /// 設置間隔時間，並更新間隔的秒數物件
        /// </summary>
        /// <param name="value"></param>
        public void setIntervalTime(float value)
        {
            interval_time = value;
            interval = new WaitForSeconds(value);
        }

        void setState(State state)
        {
            //Utils.log($"Set state {this.state} -> {state}");
            this.state = state;
        }
    }
}