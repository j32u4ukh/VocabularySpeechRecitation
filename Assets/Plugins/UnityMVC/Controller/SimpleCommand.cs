using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class SimpleCommand : MonoBehaviour, ICommand
    {
        public virtual void execute(INotification notification)
        {

        }
    }
}
