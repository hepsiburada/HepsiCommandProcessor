using Hepsi.CommandProcessor.Messaging;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAnInputChannel
    /// An <see cref="IAmAChannel"/> for reading messages from a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a>
    /// and acknowledging receipt of those messages
    /// </summary>
    public interface IAmAnInputChannel : IAmAChannel
    {
        /// <summary>
        /// Receives the specified timeout in milliseconds.
        /// </summary>
        /// <param name="timeoutinMilliseconds">The timeout in milliseconds.</param>
        /// <returns>Message.</returns>
        Message Receive(int timeoutinMilliseconds);
        /// <summary>
        /// Acknowledges the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Acknowledge(Message message);
        /// <summary>
        /// Rejects the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Reject(Message message);
        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }
}
