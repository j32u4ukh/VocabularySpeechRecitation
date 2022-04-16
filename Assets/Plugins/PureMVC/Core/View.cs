﻿//
//  PureMVC C# Multicore
//
//  Copyright(c) 2020 Saad Shams <saad.shams@puremvc.org>
//  Your reuse is governed by the Creative Commons Attribution 3.0 License
//

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace PureMVC.Core
{
    /// <summary>
    /// View（介面）關聯 Mediator 中介物件負責處理介面
    /// A Multiton <c>IView</c> implementation.
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, the <c>View</c> class assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Maintain a cache of <c>IMediator</c> instances</item>
    ///         <item>Provide methods for registering, retrieving, and removing <c>IMediators</c></item>
    ///         <item>Managing the observer lists for each <c>INotification</c> in the application</item>
    ///         <item>Providing a method for attaching <c>IObservers</c> to an <c>INotification</c>'s observer list</item>
    ///         <item>Providing a method for broadcasting an <c>INotification</c></item>
    ///         <item>Notifying the <c>IObservers</c> of a given <c>INotification</c> when it broadcast</item>
    ///     </list>
    /// </remarks>
    /// <seealso cref="PureMVC.Patterns.Mediator.Mediator"/>
    /// <seealso cref="PureMVC.Patterns.Observer.Observer"/>
    /// <seealso cref="PureMVC.Patterns.Observer.Notification"/>
    public class View: IView
    {
        /// <summary>The Multiton Key for this Core</summary>
        protected readonly string multitonKey;

        // Key: mediator 名稱, Value: mediator
        /// <summary>Mapping of Mediator names to Mediator instances</summary>
        protected readonly ConcurrentDictionary<string, IMediator> mediatorMap;

        // Key: Notification 名稱, Value: 接收到此通知時將觸發的函式們(可能是 Mediator 的 HandleNotification，也可能是 Command 的 Execute)
        /// <summary>Mapping of Notification names to Observer lists</summary>
        protected readonly ConcurrentDictionary<string, IList<IObserver>> observerMap;

        /// <summary>The Multiton View instanceMap.</summary>
        protected static readonly ConcurrentDictionary<string, Lazy<IView>> InstanceMap = new ConcurrentDictionary<string, Lazy<IView>>();

        #region View
        /// <summary>
        /// Constructs and initializes a new view
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This <c>IView</c> implementation is a Multiton, 
        ///         so you should not call the constructor 
        ///         directly, but instead call the static Multiton 
        ///         Factory method <c>View.getInstance(multitonKey, key => new View(key))</c>
        ///     </para>
        /// </remarks>
        /// <param name="key">Key of view</param>
        public View(string key)
        {
            multitonKey = key;
            InstanceMap.TryAdd(key, new Lazy<IView>(() => this));
            mediatorMap = new ConcurrentDictionary<string, IMediator>();
            observerMap = new ConcurrentDictionary<string, IList<IObserver>>();
            InitializeView();
        }

        /// <summary>
        /// <c>View</c> Multiton Factory method. 
        /// </summary>
        /// <param name="key">Key of view</param>
        /// <param name="factory">the <c>FuncDelegate</c> of the <c>IView</c></param>
        /// <returns>the instance for this Multiton key </returns>
        public static IView GetInstance(string key, Func<string, IView> factory)
        {
            return InstanceMap.GetOrAdd(key, new Lazy<IView>(() => factory(key))).Value;
        }

        /// <summary>
        /// Remove an IView instance
        /// </summary>
        /// <param name="key">multitonKey of IView instance to remove</param>
        public static void RemoveView(string key)
        {
            InstanceMap.TryRemove(key, out _);
        }

        /// <summary>
        /// Initialize the Multiton View instance.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Called automatically by the constructor, this
        ///         is your opportunity to initialize the Multiton
        ///         instance in your subclass without overriding the
        ///         constructor.
        ///     </para>
        /// </remarks>
        protected virtual void InitializeView()
        {
        }
        #endregion

        #region Mediator
        /// <summary>
        /// 讓 Facade 透過 View 註冊 Mediator，同時註冊 Mediator 感興趣的 Notification，當該 Notification 發出，觸發事先註冊的函式，並將 Notification 送過去
        /// Register an <c>IMediator</c> instance with the <c>View</c>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Registers the <c>IMediator</c> so that it can be retrieved by name,
        ///         and further interrogates the <c>IMediator</c> for its 
        ///         <c>INotification</c> interests.
        ///     </para>
        ///     <para>
        ///         If the <c>IMediator</c> returns any <c>INotification</c>
        ///         names to be notified about, an <c>Observer</c> is created encapsulating 
        ///         the <c>IMediator</c> instance's <c>handleNotification</c> method 
        ///         and registering it as an <c>Observer</c> for all <c>INotifications</c> the
        ///         <c>IMediator</c> is interested in.
        ///     </para>
        /// </remarks>
        /// <param name="mediator">the name to associate with this <c>IMediator</c> instance</param>
        public virtual void RegisterMediator(IMediator mediator)
        {
            if (mediatorMap.TryAdd(mediator.MediatorName, mediator))
            {
                mediator.InitializeNotifier(multitonKey);

                // Mediator 感興趣的 Notification 們的名稱
                var interests = mediator.ListNotificationInterests();

                if (interests.Length > 0)
                {
                    IObserver observer = new Observer(mediator.HandleNotification, mediator);

                    foreach (var interest in interests)
                    {
                        RegisterObserver(interest, observer);
                    }
                }

                // alert the mediator that it has been registered
                mediator.OnRegister();
            }
        }

        /// <summary>
        /// Check if a Mediator is registered or not
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns>whether a Mediator is registered with the given <c>mediatorName</c>.</returns>
        public virtual bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }

        /// <summary>
        /// Retrieve an <c>IMediator</c> from the <c>View</c>.
        /// </summary>
        /// <param name="mediatorName">the name of the <c>IMediator</c> instance to retrieve.</param>
        /// <returns>the <c>IMediator</c> instance previously registered with the given <c>mediatorName</c>.</returns>
        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return mediatorMap.TryGetValue(mediatorName, out var mediator) ? mediator : null;
        }

        /// <summary>
        /// Remove an <c>IMediator</c> from the <c>View</c>.
        /// </summary>
        /// <param name="mediatorName">name of the <c>IMediator</c> instance to be removed.</param>
        /// <returns>the <c>IMediator</c> that was removed from the <c>View</c></returns>
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            if (mediatorMap.TryRemove(mediatorName, out var mediator))
            {
                // Mediator 感興趣的 Notification 們的名稱
                var interests = mediator.ListNotificationInterests();

                foreach (var interest in interests)
                {
                    RemoveObserver(interest, mediator);
                }
                mediator.OnRemove();
            }
            return mediator;
        }
        #endregion

        #region Notification
        /// <summary>
        ///     Register an <c>IObserver</c> to be notified
        ///     of <c>INotifications</c> with a given name.
        /// </summary>
        /// <param name="notificationName">the name of the <c>INotifications</c> to notify this <c>IObserver</c> of</param>
        /// <param name="observer">the <c>IObserver</c> to register</param>
        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if (observerMap.TryGetValue(notificationName, out var observers))
            {
                observers.Add(observer);
            }
            else
            {
                observerMap.TryAdd(notificationName, new List<IObserver> { observer });
            }
        }

        /// <summary>
        /// Notify the <c>IObservers</c> for a particular <c>INotification</c>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         All previously attached <c>IObservers</c> for this <c>INotification</c>'s
        ///         list are notified and are passed a reference to the <c>INotification</c> in
        ///         the order in which they were registered.
        ///     </para>
        /// </remarks>
        /// <param name="notification"></param>
        public virtual void NotifyObservers(INotification notification)
        {
            // Get a reference to the observers list for this notification name
            if (observerMap.TryGetValue(notification.Name, out var observersRef))
            {
                // Copy observers from reference array to working array, 
                // since the reference array may change during the notification loop
                var observers = new List<IObserver>(observersRef);

                foreach (var observer in observers)
                {
                    observer.NotifyObserver(notification);
                }
            }
        }

        /// <summary>
        /// Remove the observer for a given notifyContext from an observer list for a given Notification name.
        /// </summary>
        /// <param name="notificationName">which observer list to remove from </param>
        /// <param name="notifyContext">remove the observer with this object as its notifyContext</param>
        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            if (observerMap.TryGetValue(notificationName, out var observers))
            {
                for (var i = 0; i < observers.Count; i++)
                {
                    if (observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }

                // Also, when a Notification's Observer list length falls to
                // zero, delete the notification key from the observer map
                if (observers.Count == 0)
                    observerMap.TryRemove(notificationName, out _);
            }
        }
        #endregion

    }
}
