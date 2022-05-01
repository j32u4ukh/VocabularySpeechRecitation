using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface IModel
    {
        void register(IProxy proxy);

        bool isExists(string name);

        IProxy get(string name);

        IProxy remove(string name);
    }
}