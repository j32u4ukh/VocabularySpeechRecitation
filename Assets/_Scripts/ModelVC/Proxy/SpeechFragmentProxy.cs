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
        public Table table;
        public string FILE_NAME;
        public string PATH;

        public SpeechFragmentProxy(string proxy_name) : base(proxy_name: proxy_name)
        {
            Utils.log($"ProxyName: {ProxyName}");
        }

        #region Life cycle
        public override void onRegister()
        {
            AppFacade.getInstance().registerCommand(ENotification.InitGroupList, () => 
            {
                return new InitGroupListCommand();
            });

            AppFacade.getInstance().sendNotification(ENotification.InitGroupList);
        }

        public override void onRemove()
        {

        } 
        #endregion
    }
}
