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
                Utils.log($"Click {KeyCode.Escape}, send Notification.OpenMainActivity");
                Facade.getInstance().sendNotification(Notification.OpenMainActivity);
            }
        }
    }
}
