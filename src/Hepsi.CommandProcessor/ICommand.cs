namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface ICommand
    /// A command is an imperative instruction to do something. We expect only one receiver of a command because it is point-to-point
    /// </summary>
    public interface ICommand : IRequest
    {
    }
}
