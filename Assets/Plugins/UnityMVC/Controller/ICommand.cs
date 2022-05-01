using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface ICommand
    {
        void execute(INotification notification);
    }
}
