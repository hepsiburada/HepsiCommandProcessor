using System;
using System.Collections.Concurrent;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.Messaging;


namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class RMQInputChannel.
    /// An <see cref="IAmAChannel"/> for reading messages from a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a>
    /// and acknowledging receipt of those messages
    /// The channel uses an AMQP application layer provided by RabbitMQ
    /// </summary>
    public class InputChannel : IAmAnInputChannel
    {
        private readonly string queueName;
        private readonly string routingKey;
        private readonly IAmAServerRequestHandler gateway;
        private readonly ConcurrentQueue<Message> queue = new ConcurrentQueue<Message>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InputChannel"/> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="routingKey">Routing key.</param>
        /// <param name="gateway">The gateway.</param>
        public InputChannel(string queueName, string routingKey, IAmAServerRequestHandler gateway)
        {
            this.queueName = queueName;
            this.routingKey = routingKey;
            this.gateway = gateway;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public ChannelName Name { get { return new ChannelName(queueName); } }

        /// <summary>
        /// Receives the specified timeoutin milliseconds.
        /// </summary>
        /// <param name="timeoutinMilliseconds">The timeoutin milliseconds.</param>
        /// <returns>Message.</returns>
        public Message Receive(int timeoutinMilliseconds)
        {
            Message message;

            if (!queue.TryDequeue(out message))
            {
                message = gateway.Receive(queueName, routingKey, timeoutinMilliseconds);
            }

            return message;
        }

        /// <summary>
        /// Acknowledges the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Acknowledge(Message message)
        {
            gateway.Acknowledge(message);
        }

        /// <summary>
        /// Rejects the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Reject(Message message)
        {
            gateway.Reject(message, false);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            queue.Enqueue(MessageFactory.CreateQuitMessage());
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Length
        {
            get { return queue.Count; }
            set { throw new NotImplementedException(); }
        }
    }
}
