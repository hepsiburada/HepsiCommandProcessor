using System;
using System.Threading.Tasks;

namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Interface IAmAMessageStore
    /// In order to provide reliability for messages sent over a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a> we
    /// store the message into a Message Store to allow later replay of those messages in the event of failure. We automatically copy any posted message into the store
    /// We provide an implementation of <see cref="IAmAMessageStore{T}"/> for Raven <see cref="RavenMessageStore"/>. Clients using other message stores should consider a Pull
    /// request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAmAMessageStore<in T> where T : Message
    {
        /// <summary>
        /// Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task Add(T message);
        /// <summary>
        /// Gets the specified message identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>Task&lt;Message&gt;.</returns>
        Task<Message> Get(Guid messageId);
    }
}
