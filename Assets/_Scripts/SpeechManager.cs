using System;
using UnityEngine;
using UnityEngine.UI;
using vts;
using vts.mvc;
using SpeechLib;
using System.Collections.Generic;
using System.Collections;

namespace vts
{

    public class SpeechManager : MonoBehaviour
    {
        private static SpeechManager instance = null;
        SpVoice voice;
        ISpeechObjectTokens tokens;

        [SerializeField] FantomLib.TextToSpeechController controller;

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
#if UNITY_EDITOR
            voice = new SpVoice();
            tokens = voice.GetVoices(string.Empty, string.Empty);

#elif UNITY_ANDROID
            controller.OnStatus.AddListener(onStatus);
#endif

            //StartCoroutine(reciteByMode(ReciteMode.Spelling, content: "content", language: SystemLanguage.English));
            StartCoroutine(reciteContent(vocabulary: "content", 
                                         description: "內容", 
                                         target: SystemLanguage.English, 
                                         describe: SystemLanguage.ChineseTraditional,
                                         ReciteMode.Word, ReciteMode.Word, ReciteMode.Spelling, ReciteMode.Interval, ReciteMode.Description));
        }

        public static SpeechManager getInstance()
        {
            return instance;
        }

        public void onStatus(string message)
        {
            Debug.Log($"[SpeechManager] OnStatus | message: {message}");
        }

        /// <summary>
        /// SpVoice    /// 
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
#if UNITY_EDITOR

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
                default:
                    voice.Voice = tokens.Item(0);
                    break;
            }

#elif UNITY_ANDROID
        string language_code = Utils.getLanguageCode(language: language);

        if(language_code != null)
        {
            controller.Locale = language_code;
        }
        else
        {
            controller.Locale = Utils.getLanguageCode(language: SystemLanguage.ChineseTraditional);
        }
#endif
        }

        // TODO: 實作說完事件監聽，再播放下一句
        // TODO: 找出兩次說話間隔時間的設定
        public void speak(string content, float speed = 1.0f)
        {
#if UNITY_EDITOR
            // SpVoice 的語速設定範圍 -10(最慢) ~ 10(最快)
            // https://docs.microsoft.com/zh-tw/dotnet/api/system.speech.synthesis.speechsynthesizer.rate?view=netframework-4.8
            voice.Rate = Mathf.Clamp((int)Math.Round(speed) - 1, -10, 10);

            voice.Speak(content);

#elif UNITY_ANDROID
            controller.StartSpeech(content);
#endif
        }

        /// <summary>
        /// SpVoice 的語速設定範圍 -10(最慢) ~ 10(最快)
        /// FantomLib 的語速設定範圍 0.5f(最慢) ~ 2.0f(最快)
        /// </summary>
        /// <param name="speed"></param>
        public void setSpeed(float speed)
        {
#if UNITY_EDITOR
            // https://docs.microsoft.com/zh-tw/dotnet/api/system.speech.synthesis.speechsynthesizer.rate?view=netframework-4.8
            voice.Rate = Mathf.Clamp((int)Math.Round(speed) - 1, -10, 10);

#elif UNITY_ANDROID
            controller.Speed = speed;
#endif
        }

        public void startReciteContent(VocabularyNorm vocab, SystemLanguage target, SystemLanguage describe = SystemLanguage.ChineseTraditional)
        {
            StartCoroutine(reciteContent(vocab: vocab, target: target, describe: describe));
        }

        IEnumerator reciteContent(VocabularyNorm vocab, SystemLanguage target, SystemLanguage describe = SystemLanguage.ChineseTraditional)
        {
            setLanguage(language: target);
            speak(content: vocab.vocabulary);
            yield return new WaitForSeconds(1.0f);

            setLanguage(language: describe);
            speak(content: vocab.description);
        }

        IEnumerator reciteContent(string vocabulary, string description, SystemLanguage target, SystemLanguage describe, params ReciteMode[] modes)
        {
            foreach(ReciteMode mode in modes)
            {
                switch (mode)
                {
                    case ReciteMode.Word:
                        setLanguage(language: target);
                        speak(content: vocabulary);
                        break;

                    case ReciteMode.Description:
                        setLanguage(language: describe);
                        speak(content: description);
                        break;

                    case ReciteMode.Spelling:
                        setLanguage(language: target);

                        foreach (string s in getSpelling(content: vocabulary, language: target))
                        {
                            speak(content: s);
                        }
                        break;

                    case ReciteMode.Interval:
                        yield return new WaitForSeconds(Config.interval_time);
                        break;
                }
            }
        }

        IEnumerator reciteByMode(ReciteMode mode, string content = null, SystemLanguage language = SystemLanguage.ChineseTraditional)
        {
            // 只有 ReciteMode.Interval 模式允許 content 是 null，若不是則直接返回
            if (content == null && mode != ReciteMode.Interval)
            {
                yield break;
            }

            switch (mode)
            {
                case ReciteMode.Word:
                case ReciteMode.Description:
                    speak(content: content);
                    break;
                case ReciteMode.Spelling:
                    foreach (string s in getSpelling(content: content, language: language))
                    {
                        speak(content: s);
                    }
                    break;
                case ReciteMode.Interval:
                    yield return new WaitForSeconds(Config.interval_time);
                    break;
            }
        }

        IEnumerable<string> getSpelling(string content, SystemLanguage language = SystemLanguage.ChineseTraditional)
        {
            switch (language)
            {
                case SystemLanguage.English:
                default:
                    foreach (char c in content)
                    {
                        yield return c.ToString();
                    }
                    break;
            }
        }
    }
}