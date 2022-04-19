using PureMVC.Interfaces;

namespace vts.mvc
{
    public class MainActivityMediator : Mediator
    {
        public MainActivityMediator(string mediator_name, object component) : base(mediator_name: mediator_name, component: component)
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


