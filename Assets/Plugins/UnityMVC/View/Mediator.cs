using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Mediator : MonoBehaviour, IMediator
    {
        protected string mediator_name = null;

        /// <summary>
        /// 預設在 Awake 就利用物件名稱來對 Mediator 做註冊，若需要複用等不希望在 Awake 就註冊的需求，
        /// 可以繼承 Mediator 之後，將 Awake 複寫，再自行命名與註冊即可
        /// </summary>
        public virtual void Awake()
        {
            register(name: GetType().Name);
        }

        public virtual void OnDestroy()
        {
            Facade.getInstance().releaseMediator(mediator_name: mediator_name);
        }

        public virtual void register(string name)
        {
            mediator_name = name;
            Facade.getInstance().registerMediator(this);
        }

        public string getName()
        {
            return mediator_name;
        }

        public virtual void onRegister()
        {

        }

        public virtual void onRelease()
        {

        }

        public virtual IEnumerable<string> subscribeNotifications()
        {
            return Facade.No_TIFICATION;
        }

        public virtual void onNotificationListener(INotification notification)
        {
            
        }
    }
}
