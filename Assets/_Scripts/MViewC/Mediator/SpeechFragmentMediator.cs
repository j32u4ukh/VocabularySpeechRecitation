using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public class SpeechFragmentMediator : Mediator
    {
        public SpeechFragmentMediator(string mediator_name, object component) : base(mediator_name: mediator_name, component: component)
        {

        }

        public override ENotification[] registerNotifications()
        {
            return new ENotification[] { ENotification.GroupListLoaded };
        }

        public override void onNotificationListener(INotification notification)
        {
            ENotification en = AppFacade.transNameToEnum(name: notification.Name);

            switch (en)
            {
                case ENotification.GroupListLoaded:
                    displayGroupList();
                    break;
            }
        }

        #region MyRegion
        public override void onRegister()
        {
            Utils.log();
        }

        public override void onRemove()
        {

        } 
        #endregion

        void displayGroupList()
        {

        }
    }
}
