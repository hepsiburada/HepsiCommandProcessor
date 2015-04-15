using System;
using System.Threading.Tasks;
using Hepsi.CommandProcessor.Messaging;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Interface IAmASendMessageGateway
    /// Abstracts away the Application Layer used to push messages onto a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a>
    /// Usually clients do not need to instantiate as access is via an <see cref="IAmAChannel"/> derived class.
    /// We provide the following default gateway applications
    /// <list type="bullet">
    /// <item>AMQP</item>
    /// <item>RESTML</item>
    /// </list>
    /// </summary>
    public interface IAmAClientRequestHandler : IDisposable
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task Send(Message message);
    }
}
