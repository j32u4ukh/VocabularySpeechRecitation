using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Proxy : IProxy
    {
        string name = null;
        object data = null;

        public Proxy()
        {

        }

        #region Life cycle
        public virtual void onRegister()
        {

        }

        public virtual void onExpulsion()
        {

        } 
        #endregion

        public void setName(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }

        public void setData(object data)
        {
            this.data = data;
        }

        public object getData()
        {
            return data;
        }
    }
}
