using System.Collections;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class SpeechActivityCommander : Commander 
    {
        private void Update()
        {
            // 對應手機上的返回鍵
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SpeechManager.getInstance().stop();
                SpeechManager.getInstance().release();
                Facade.getInstance().sendNotification(Notification.StopSpeaking);
                Facade.getInstance().sendNotification(Notification.OpenMainActivity);
            }
        }
    }
}
