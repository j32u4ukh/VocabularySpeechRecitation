using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class MacroCommand : MonoBehaviour, ICommand
    {
        /// <summary>List of subcommands</summary>
        public readonly IList<Func<ICommand>> commands;

        public MacroCommand()
        {
            commands = new List<Func<ICommand>>();
            initMacroCommand();
        }

        protected virtual void initMacroCommand()
        {
        }

        protected void addCommand(Func<ICommand> command)
        {
            commands.Add(command);
        }

        public virtual void execute(INotification notification)
        {
            ICommand command;

            foreach (Func<ICommand> func in commands)
            {
                command = func();
                command.execute(notification);
            }

            commands.Clear();
        }
    }
}
