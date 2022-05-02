using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface IView
    {
        void register(IMediator mediator);

        bool isExists(string name);

        IMediator get(string name);

        IMediator expulsion(string name);
    }
}
