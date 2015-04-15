using Hepsi.CommandProcessor.Messaging;


namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAnOutputChannel
    /// An <see cref="IAmAChannel"/> for pushing messages onto a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a>
    /// </summary>
    public interface IAmAnOutputChannel : IAmAChannel
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Send(Message message);
    }
}
