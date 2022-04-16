using System;
using UnityEngine;
using UnityEngine.UI;

public class SpeechManager : MonoBehaviour
{
    public FantomLib.TextToSpeechController controller;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        //controller.OnStatus.AddListener(OnStatus);

        button.onClick.AddListener(()=> 
        {
            Debug.Log($"Locale: {controller.Locale}");
            controller.StartSpeech("こんねねー 五期生オレンジ担当、桃鈴ねねです");
        });

        Excuter excuter = new Excuter();
        ProxyTest test = new ProxyTest(excuter.Print);
        Notify notify = new Notify();
        notify.message = "Hello PureMVC";
        test.Print(notify);
    }

    public void OnStatus(string message)
    {
        Debug.Log($"[SpeechManager] OnStatus | message: {message}");
    }
}

public interface INotify
{
    public string message { get; set; }
}

class Notify : INotify
{
    public string message { get; set; }
}

class ProxyTest
{
    Action<INotify> notifyMethod;

    public ProxyTest(Action<INotify> notifyMethod)
    {
        this.notifyMethod = notifyMethod;
    }

    public void Print(INotify notify)
    {
        notifyMethod(notify);
    }
}

class Excuter
{
    public void Print(INotify notify)
    {
        Debug.Log(notify.message);
    }
}