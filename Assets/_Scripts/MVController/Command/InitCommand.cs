using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public class InitCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            Utils.log("Init");
        }
    }
}


