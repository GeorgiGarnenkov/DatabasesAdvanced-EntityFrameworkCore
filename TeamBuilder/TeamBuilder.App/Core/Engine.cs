using System;

namespace TeamBuilder.App.Core
{
    public class Engine
    {
        private readonly CommandDispatcher _dispatcher;

        public Engine(CommandDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    string output = this._dispatcher.Dispatch(input);

                    Console.WriteLine(output);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetBaseException().Message);
                }
            }
        }
    }
}