using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using vts.data_operation;

namespace vts.mvc
{

    public class SpeechFragmentProxy : Proxy
    {
        private Table table;
        private string FILE_NAME;
        private string PATH;

        public SpeechFragmentProxy(string proxy_name) : base(proxy_name: proxy_name)
        {
            Utils.log($"ProxyName: {ProxyName}");
            int tag = 100 * (int)Config.target + (int)Config.describe;

            switch (tag)
            {
                // English -> Chinese
                case 1006:
                default:
                    FILE_NAME = "EnTw";
                    break;
            }

            PATH = Path.Combine(Application.streamingAssetsPath, "vocabulary", $"{FILE_NAME}.csv");
        }

        #region Life cycle
        public override void onRegister()
        {
            table = new Table();
            _ = table.loadAsync(path: PATH);

            table.onFileLoaded += () => 
            {
                AppFacade.getInstance().sendNotification(ENotification.GroupListLoaded);            
            };
        }

        public override void onRemove()
        {

        } 
        #endregion

        public void addLoadedListener(Action callback)
        {
            table.onFileLoaded += callback;
        }
    }
}
