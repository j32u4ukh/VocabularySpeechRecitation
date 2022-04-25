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
            return NO_TIFICATION;
        }

        public override void handleNotification(INotification notification)
        {
            
        }

        public override void onRegister()
        {
            Utils.log();
        }

        public override void onRemove()
        {
            
        }
    }
}
