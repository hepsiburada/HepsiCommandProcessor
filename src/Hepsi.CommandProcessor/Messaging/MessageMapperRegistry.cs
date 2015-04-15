using System;
using System.Collections;
using System.Collections.Generic;


namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Class MessageMapperRegistry
    /// In order to use a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a> approach we require you to provide
    /// a <see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapper"/> to map between <see cref="Command"/> or <see cref="Event"/> and a <see cref="Message"/> 
    /// registered via <see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapperRegistry"/>
    /// This is a default implementation of<see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapperRegistry"/> which is suitable for most usages, the interface is provided mainly for testing
    /// </summary>
    public class MessageMapperRegistry : IAmAMessageMapperRegistry, IEnumerable<KeyValuePair<Type, Type>>
    {
        private readonly IAmAMessageMapperFactory messageMapperFactory;
        readonly Dictionary<Type, Type> messageMappers = new Dictionary<Type, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageMapperRegistry"/> class.
        /// </summary>
        /// <param name="messageMapperFactory">The message mapper factory.</param>
        public MessageMapperRegistry(IAmAMessageMapperFactory messageMapperFactory)
        {
            this.messageMapperFactory = messageMapperFactory;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="TRequest">The type of the t request.</typeparam>
        /// <returns>IAmAMessageMapper&lt;TRequest&gt;.</returns>
        public IAmAMessageMapper<TRequest> Get<TRequest>() where TRequest : class, IRequest
        {
            if (!messageMappers.ContainsKey(typeof(TRequest)))
            {
                return null;
            }

            var messageMapperType = messageMappers[typeof(TRequest)];

            return (IAmAMessageMapper<TRequest>)messageMapperFactory.Create(messageMapperType);
        }

        //support object initializer
        /// <summary>
        /// Adds the specified message type.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageMapper">The message mapper.</param>
        public void Add(Type messageType, Type messageMapper)
        {
            messageMappers.Add(messageType, messageMapper);
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TRequest">The type of the t request.</typeparam>
        /// <typeparam name="TMessageMapper">The type of the t message mapper.</typeparam>
        /// <exception cref="System.ArgumentException"></exception>
        public void Register<TRequest, TMessageMapper>()
            where TRequest : class, IRequest
            where TMessageMapper : class, IAmAMessageMapper<TRequest>
        {
            if (messageMappers.ContainsKey(typeof(TRequest)))
            {
                throw new ArgumentException(string.Format("Message type {0} alread has a mapper; only one mapper can be registred per type", typeof(TRequest).Name));
            }

            messageMappers.Add(typeof(TRequest), typeof(TMessageMapper));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<Type, Type>> GetEnumerator()
        {
            return messageMappers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
