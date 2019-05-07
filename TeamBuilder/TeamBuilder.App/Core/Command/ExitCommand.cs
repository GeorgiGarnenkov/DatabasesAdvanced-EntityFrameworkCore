using System;
using TeamBuilder.App.Utilities;

namespace TeamBuilder.App.Core.Command
{
    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(0, args);

            Environment.Exit(0);

            return "Bye!";
        }
    }
}