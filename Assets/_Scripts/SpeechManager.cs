using System;
using UnityEngine;
using UnityEngine.UI;
using vts;
using vts.mvc;
using SpeechLib;

public class SpeechManager : MonoBehaviour
{
    public FantomLib.TextToSpeechController controller;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        controller.OnStatus.AddListener(OnStatus);
#endif

        button.onClick.AddListener(()=> 
        {
#if UNITY_EDITOR
            SpVoice voice = new SpVoice();
            ISpeechObjectTokens tokens = voice.GetVoices(string.Empty, string.Empty);

            int i, len = tokens.Count;

            /// 會因不同電腦有安裝的語言套件而有所不同，不能一概而論
            /// 0: Microsoft Hanhan Desktop - Chinese (Taiwan)
            /// 1: Microsoft Zira Desktop - English (United States)
            /// 2: Microsoft Haruka Desktop - Japanese
            /// 3: Microsoft David Desktop - English (United States)
            for (i = 0; i < len; i++)
            {
                Utils.log($"{i}: {tokens.Item(i).GetDescription()}");
            }

            voice.Voice = tokens.Item(2);
            voice.Speak("こんねねー 五期生オレンジ担当、桃鈴ねねです");

#elif UNITY_ANDROID
            Debug.Log($"Locale: {controller.Locale}");
            controller.StartSpeech("こんねねー 五期生オレンジ担当、桃鈴ねねです");
#endif
        });

        AppFacade.getInstance().init();
    }

    public void OnStatus(string message)
    {
        Debug.Log($"[SpeechManager] OnStatus | message: {message}");
    }
}