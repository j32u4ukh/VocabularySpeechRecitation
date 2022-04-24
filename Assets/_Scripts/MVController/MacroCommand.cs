using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public abstract class MacroCommand : PureMVC.Patterns.Command.MacroCommand
    {
        /// <summary>
        /// Initialize the <c>MacroCommand</c>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         In your subclass, override this method to 
        ///         initialize the <c>MacroCommand</c>'s <i>SubCommand</i>  
        ///         list with <c>ICommand</c> class references like
        ///         this:
        ///     </para>
        ///     <example>
        ///         <code>
        ///             override void InitializeMacroCommand() 
        ///             {
        ///                 AddSubCommand(() => new com.me.myapp.controller.FirstCommand());
        ///                 AddSubCommand(() => new com.me.myapp.controller.SecondCommand());
        ///                 AddSubCommand(() => new com.me.myapp.controller.ThirdCommand());
        ///             }
        ///         </code>
        ///     </example>
        ///     <para>
        ///         Note that <i>SubCommand</i>s may be any <c>ICommand</c> implementor,
        ///         <c>MacroCommand</c>s or <c>SimpleCommands</c> are both acceptable.
        ///     </para>
        /// </remarks>
        protected override void InitializeMacroCommand()
        {
            init();
        }

        /// <summary>
        /// 多次呼叫 AddSubCommand，將多個 SimpleCommand 或 MacroCommand 加入待執行的命令們
        /// </summary>
        public abstract void init();

        /// <summary>
        /// Add a <c>SubCommand</c>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The <i>SubCommands</i> will be called in First In/First Out (FIFO)
        ///         order.
        ///     </para>
        /// </remarks>
        /// <param name="factory">a reference to the <c>FuncDelegate</c> of the <c>ICommand</c>.</param>
        protected void addSubCommand(Func<PureMVC.Interfaces.ICommand> factory)
        {
            AddSubCommand(factory);
        }

        public virtual void execute(PureMVC.Interfaces.INotification notification)
        {
            Execute(notification);
        }

        /// <summary>
        /// Execute this <c>MacroCommand</c>'s <i>SubCommands</i>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The <i>SubCommands</i> will be called in First In/First Out (FIFO)
        ///         order.
        ///     </para>
        /// </remarks>
        /// <param name="notification">the <c>INotification</c> object to be passed to each <i>SubCommand</i>.</param>
        public override void Execute(PureMVC.Interfaces.INotification notification)
        {
            while (subcommands.Count > 0)
            {
                var factory = subcommands[0];
                var commandInstance = factory();
                commandInstance.InitializeNotifier(MultitonKey);
                commandInstance.Execute(notification);
                subcommands.RemoveAt(0);
            }
        }
    }
}



