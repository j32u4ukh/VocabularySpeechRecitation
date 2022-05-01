using PureMVC.Interfaces;
using vts.mvc;

namespace vts
{
    public class InitCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            Utils.log("Init");
            MainActivity activity = notification.Body as MainActivity;
            MainActivityMediator mediator = new MainActivityMediator(mediator_name: MediatorName.MainActivity, 
                                                                     activity: activity.gameObject);
            AppFacade.getInstance().registerMediator(mediator);
        }
    }
}


