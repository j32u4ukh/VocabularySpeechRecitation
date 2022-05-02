using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface IController
    {
        void register(ICommand commnad);

        bool isExists(string name);

        ICommand expulsion(string name);
    }
}
