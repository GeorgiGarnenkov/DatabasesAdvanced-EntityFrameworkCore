namespace TeamBuilder.App.Core.Command
{
    public interface ICommand
    {
        string Execute(string[] args);
    }
}