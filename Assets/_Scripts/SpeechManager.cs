using System;
using UnityEngine;
using UnityEngine.UI;
using vts;
using vts.mvc;
using SpeechLib;
using System.Collections.Generic;
using System.Collections;

public class SpeechManager : MonoBehaviour
{
    private static SpeechManager instance = null;
    SpVoice voice;
    ISpeechObjectTokens tokens;

    [SerializeField] FantomLib.TextToSpeechController controller;

    private void Awake()
    {
        if(instance == null)
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

    public void speak(string content)
    {
#if UNITY_EDITOR
        voice.Speak(content);

#elif UNITY_ANDROID
        controller.StartSpeech(content);
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
}