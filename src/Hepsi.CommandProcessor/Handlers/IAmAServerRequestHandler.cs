using System;
using Hepsi.CommandProcessor.Messaging;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Interface IAmAReceiveMessageGateway
    /// </summary>
    public interface IAmAServerRequestHandler : IDisposable
    {
        /// <summary>
        /// Receives the specified queue name.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="timeoutInMilliseconds">The timeout in milliseconds.</param>
        /// <returns>Message.</returns>
        Message Receive(string queueName, string routingKey, int timeoutInMilliseconds);
        /// <summary>
        /// Acknowledges the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Acknowledge(Message message);
        /// <summary>
        /// Rejects the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        void Reject(Message message, bool requeue);
        /// <summary>
        /// Purges the specified queue name.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        void Purge(string queueName);
    }
}
