using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    /// <summary>
    /// PureMVC 的基本使用的呼叫流程梳理
    /// 1. 定義自己的 Facade 類，繼承 Facade 類，提供這個類的單例模式屬性 Instance
    /// 2. 使用自己的 Facade 類物件的 SendNotification 方法傳送通知，對這個方法進行封裝。
    /// 3. 一定存在一個註冊通知的函式，否則自己定義的通知無法執行。在自己定義的 Facade 函式中重寫 InitializeController 方法，在這個方法中呼叫 RegisterCommand 函式註冊通知。
    /// 4. 定義剛才註冊通知時返回的 Command 類。自定義的 Command 類繼承自 SimpleCommand 類或者 MacroCommand 類（都實現了 ICommand 介面）。
    ///    SimpleCommand 必須重寫 Execute 方法，當前 Command 需要執行的邏輯程式碼就定義在這個方法中；
    ///    MacroCommand 必須重寫 InitializeMacroCommand 方法，它持有一個 IList<Func<ICommand>> 型別的 subCommands 變數，
    ///    MacroCommand 可以持有多個 SimpleCommand 或者 MacroCommand，都儲存在 subCommands 變數中，它的 Execute 方法已經定義好了不用重寫，
    ///    Execute 函式會依次執行其持有的所有 SimpleCommand 和 MacroCommand，在 InitializeMacroCommand 方法中通過 AddSubCommand 方法將 Command 加入 subCommands 變數即可。
    /// </summary>
    public class AppFacade : PureMVC.Patterns.Facade.Facade
    {
        private static string KEY = "MainFacade";

        public AppFacade() : base(key: KEY)
        {

        }

        // 有看到有的寫法是返回 IFacade，不知道有什麼優缺點？
        public static AppFacade getInstance()
        {
            return GetInstance(KEY, key => new AppFacade()) as AppFacade;
        }

        /// <summary>
        /// 初始化controller相關內容
        /// </summary>
        protected override void InitializeController()
        {
            //可以保留，父類中初始化時new了一個controller
            base.InitializeController();

            // 命令和通知繫結的邏輯
            // 註冊通知，類似於委託，在函式中返回一個命令，
            registerCommand(ENotification.Init, () =>
            {
                return new InitCommand();
            });
        }

        /// <summary>
        /// 啟動命令的函式，其他函式呼叫這個函式啟動命令
        /// </summary>
        public void init()
        {
            sendNotification(ENotification.Init);
        }

        #region Model
        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c> by name.
        /// </summary>
        /// <param name="proxy">the <c>IProxy</c> instance to be registered with the <c>Model</c>.</param>
        public void registerProxy(PureMVC.Interfaces.IProxy proxy)
        {
            RegisterProxy(proxy);
        } 
        #endregion

        #region View
        /// <summary>
        /// Register a <c>IMediator</c> with the <c>View</c>.
        /// </summary>
        /// <param name="mediator">a reference to the <c>IMediator</c></param>
        public void registerMediator(PureMVC.Interfaces.IMediator mediator)
        {
            RegisterMediator(mediator);
        } 
        #endregion

        #region Controller
        /// <summary>
        /// Register an <c>ICommand</c> with the <c>Controller</c> by Notification name.
        /// </summary>
        /// <param name="notificationName">the name of the <c>INotification</c> to associate the <c>ICommand</c> with</param>
        /// <param name="commandFunc">a reference to the Class of the <c>ICommand</c></param>
        public void registerCommand(ENotification notification, Func<PureMVC.Interfaces.ICommand> commandFunc)
        {
            RegisterCommand(notification.ToString(), commandFunc);
        }
        #endregion

        #region Notification
        public void sendNotification(ENotification notification, object body = null, string type = null)
        {
            SendNotification(notification.ToString(), body, type);
        } 
        #endregion
    }

}