using System.Collections.Concurrent;
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

        public virtual bool tryGet(string name, out IProxy proxy)
        {
            bool is_exists = proxies.TryGetValue(name, out proxy);

            if (!is_exists)
            {
                Debug.LogWarning($"[{GetType().Name}] get | Get IProxy({name}) failed.");
            }

            return is_exists;
        }

        public virtual bool isExists(string name)
        {
            return proxies.ContainsKey(name);
        }

        public virtual IProxy release(string name)
        {
            if (proxies.TryRemove(name, out IProxy proxy))
            {
                proxy.onRelease();
                return proxy;
            }

            Debug.LogWarning($"[{GetType().Name}] expulsion | IProxy({name}) is not exists.");

            return null;
        }
    }
}
