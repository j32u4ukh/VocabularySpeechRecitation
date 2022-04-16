using UnityEngine;
using UnityEngine.UI;

public class SpeechManager : MonoBehaviour
{
    public FantomLib.TextToSpeechController controller;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        controller.OnStatus.AddListener(OnStatus);

        button.onClick.AddListener(()=> 
        {
            Debug.Log($"Locale: {controller.Locale}");
            controller.StartSpeech("こんねねー 五期生オレンジ担当、桃鈴ねねです");
        });
    }

    public void OnStatus(string message)
    {
        Debug.Log($"[SpeechManager] OnStatus | message: {message}");
    }
}
