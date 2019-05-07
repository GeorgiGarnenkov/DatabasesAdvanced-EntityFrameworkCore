using TeamBuilder.App.Core;
using TeamBuilder.Data;

namespace TeamBuilder.App
{
   public class StartUp
    {
        public static void Main()
        {
            using (var context = new TeamBuilderContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            Engine engine = new Engine(new CommandDispatcher());
            engine.Run();
        }
    }
}
