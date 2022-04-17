namespace vts.mvc
{
    public abstract class Mediator : PureMVC.Patterns.Mediator.Mediator
    {
        protected readonly static ENotification[] NO_TIFICATION = new ENotification[0];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator_name"></param>
        /// <param name="component"> 對應一個 UI 組件</param>
        public Mediator(string mediator_name = null, object component = null) : base(mediatorName: NAME, viewComponent: component)
        {
            MediatorName = mediator_name ?? this.GetType().Name;
        }

        #region PureMVC.Patterns.Mediator.Mediator
        /// <summary>
        /// Mediator 感興趣的 Notification 們的名稱
        /// List the <c>INotification</c> names this
        /// <c>Mediator</c> is interested in being notified of.
        /// </summary>
        /// <returns>the list of <c>INotification</c> names</returns>
        public override string[] ListNotificationInterests()
        {
            ENotification[] notifications = registerNotifications();
            int len = notifications.Length;

            if (len.Equals(0))
            {
                return new string[0];
            }

            string[] interests = new string[len];

            for(int i = 0; i < len; i++)
            {
                Utils.log($"interests: {notifications[i]}");
                interests[i] = notifications[i].ToString();
            }

            return interests;
        }

        /// <summary>
        /// Handle <c>INotification</c>s.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Typically this will be handled in a switch statement,
        ///         with one 'case' entry per <c>INotification</c>
        ///         the <c>Mediator</c> is interested in.
        ///     </para>
        /// </remarks>
        /// <param name="notification"></param>
        public override void HandleNotification(PureMVC.Interfaces.INotification notification)
        {
            handleNotification(notification);
        }

        /// <summary>
        /// Called by the View when the Mediator is registered
        /// </summary>
        public override void OnRegister()
        {
            onRegister();
        }

        /// <summary>
        /// Called by the View when the Mediator is removed
        /// </summary>
        public override void OnRemove()
        {
            onRemove();
        }
        #endregion

        /// <summary>
        /// Mediator 感興趣的 Notification 們的名稱
        /// List the <c>INotification</c> names this
        /// <c>Mediator</c> is interested in being notified of.
        /// </summary>
        /// <returns>the list of <c>INotification</c> names</returns>
        public abstract ENotification[] registerNotifications();

        /// <summary>
        /// Handle <c>INotification</c>s.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Typically this will be handled in a switch statement,
        ///         with one 'case' entry per <c>INotification</c>
        ///         the <c>Mediator</c> is interested in.
        ///     </para>
        /// </remarks>
        /// <param name="notification"></param>
        public abstract void handleNotification(PureMVC.Interfaces.INotification notification);

        /// <summary>
        /// Called by the View when the Mediator is registered
        /// </summary>
        public abstract void onRegister();

        /// <summary>
        /// Called by the View when the Mediator is removed
        /// </summary>
        public abstract void onRemove();
    }
}
