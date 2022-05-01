using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using vts.data_operation;

namespace vts.mvc
{
    public class InitGroupListCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            int tag = 100 * (int)Config.target + (int)Config.describe;
            SpeechFragmentProxy proxy = AppFacade.getInstance().getProxy(proxy_name: vts.ProxyName.GroupList) as SpeechFragmentProxy;

            switch (tag)
            {
                // English -> Chinese
                case 1006:
                default:
                    proxy.FILE_NAME = "EnTw";
                    break;
            }

            proxy.PATH = Path.Combine(Application.streamingAssetsPath, "vocabulary", $"{proxy.FILE_NAME}.csv");
            Utils.log($"tag: {tag}, PATH: {proxy.PATH}");

            proxy.table = new Table();
            _ = proxy.table.loadAsync(path: proxy.PATH);

            proxy.table.onFileLoaded += () =>
            {
                Utils.log($"#group: {proxy.table.getRowNumber()}");
                AppFacade.getInstance().sendNotification(ENotification.GroupListLoaded);
            };
        }
    }
}