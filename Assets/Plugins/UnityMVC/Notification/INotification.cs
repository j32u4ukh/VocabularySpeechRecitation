using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface INotification
    {
        void setName(string name);

        public string getName();

        void setHeader(string header);

        public string getHeader();

        void setBody(object body);

        public object getBody();

        string toString();
    }
}
