using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class MainActivityMediator : Mediator
    {
        GameObject obj;

        public MainActivityMediator(GameObject obj) : base(mediator_name: null, component: obj)
        {
          
        }

        public override ENotification[] registerNotifications()
        {
            return NO_TIFICATION;
        }

        public override void handleNotification(INotification notification)
        {
            // 根據通知的名稱作相應的處理
            switch (notification.Name)
            {
                default:
                    Debug.Log(obj.name);
                    break;
            }
        }

        public override void onRegister()
        {

        }

        public override void onRemove()
        {

        }
    }
}


