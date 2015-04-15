using System;
using System.Collections.Generic;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmASubscriberRegistry
    /// In order to map an <see cref="IHandleRequests"/> to a <see cref="Command"/> or an <see cref="Event"/> we need you to register the association
    /// via the <see cref="SubscriberRegistry"/>
    /// The default implementation of <see cref="SubscriberRegistry"/> is usable in most instances and this is provided for testing
    /// </summary>
    public interface IAmASubscriberRegistry
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable&lt;Type&gt;.</returns>
        IEnumerable<Type> Get<T>() where T : class, IRequest;
        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TRequest">The type of the t request.</typeparam>
        /// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
        void Register<TRequest, TImplementation>()
            where TRequest : class, IRequest
            where TImplementation : class, IHandleRequests<TRequest>;
    }
}
