using System.Collections.Generic;
using UnityMVC;

namespace VTS
{
    public class MainActivityCommand : Command
    {
        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] { 
                Notification.InitSpeechActivity
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.InitSpeechActivity:
                    initGroupList();
                    break;
            }
        }

        void initGroupList()
        {
            int tag = 100 * (int)Config.target + (int)Config.describe;
            //SpeechFragmentProxy proxy = Facade.getInstance().getProxy(proxy_name: vts.ProxyName.GroupList) as SpeechFragmentProxy;

            //switch (tag)
            //{
            //    // English -> Chinese
            //    case 1006:
            //    default:
            //        proxy.FILE_NAME = "EnTw";
            //        break;
            //}

            //proxy.PATH = Path.Combine(Application.streamingAssetsPath, "vocabulary", $"{proxy.FILE_NAME}.csv");
            //Utils.log($"tag: {tag}, PATH: {proxy.PATH}");

            //proxy.table = new Table();
            //_ = proxy.table.loadAsync(path: proxy.PATH);

            //proxy.table.onFileLoaded += () =>
            //{
            //    Utils.log($"#group: {proxy.table.getRowNumber()}");
            //    Facade.getInstance().sendNotification(Notification.GroupListLoaded);
            //};
        }
    }
}