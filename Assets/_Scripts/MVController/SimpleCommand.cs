using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public abstract class SimpleCommand : PureMVC.Patterns.Command.SimpleCommand
    {
        public override void Execute(PureMVC.Interfaces.INotification notification)
        {
            execute(notification);
        }

        public abstract void execute(PureMVC.Interfaces.INotification notification);
    }
}
