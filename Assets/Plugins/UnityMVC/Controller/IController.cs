using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface IController
    {
        void register(ICommander commnad);

        bool isExists(string name);

        ICommander expulsion(string name);
    }
}
