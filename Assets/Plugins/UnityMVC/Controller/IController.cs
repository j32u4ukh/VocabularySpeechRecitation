using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface IController
    {
        void register(string name, Func<ICommand> func);

        bool isExists(string name);

        void execute(INotification notification);

        void remove(string name);
    }
}
