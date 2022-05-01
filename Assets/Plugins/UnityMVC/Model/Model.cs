using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Model : IModel
    {
        protected readonly ConcurrentDictionary<string, IProxy> proxies;

        public Model()
        {
            proxies = new ConcurrentDictionary<string, IProxy>();
        }

        public virtual void register(IProxy proxy)
        {
            proxies[proxy.getName()] = proxy;
            proxy.onRegister();
        }

        public virtual IProxy get(string name)
        {
            return proxies.TryGetValue(name, out IProxy proxy) ? proxy : null;
        }

        public virtual bool isExists(string name)
        {
            return proxies.ContainsKey(name);
        }

        public virtual IProxy remove(string name)
        {
            if (proxies.TryRemove(name, out IProxy proxy))
            {
                proxy.onRemove();
            }

            return proxy;
        }
    }
}
