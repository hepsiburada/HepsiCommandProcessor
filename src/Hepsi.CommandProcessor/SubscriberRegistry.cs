using System;
using System.Collections;
using System.Collections.Generic;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class SubscriberRegistry.
    /// In order to map an <see cref="IHandleRequests"/> to a <see cref="Command"/> or an <see cref="Event"/> we need you to register the association
    /// via the <see cref="SubscriberRegistry"/>
    /// The default implementation of <see cref="SubscriberRegistry"/> is usable in most instances and this is provided for testing
    /// </summary>
    public class SubscriberRegistry : IAmASubscriberRegistry, IEnumerable<KeyValuePair<Type, List<Type>>>
    {
        private readonly Dictionary<Type, List<Type>> observers = new Dictionary<Type, List<Type>>();

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="TRequest">The type of the t request.</typeparam>
        /// <returns>IEnumerable&lt;Type&gt;.</returns>
        public IEnumerable<Type> Get<TRequest>() where TRequest : class, IRequest
        {
            var observed = observers.ContainsKey(typeof(TRequest));

            return observed ? observers[typeof(TRequest)] : new List<Type>();
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TRequest">The type of the t request.</typeparam>
        /// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
        public void Register<TRequest, TImplementation>()
            where TRequest : class, IRequest
            where TImplementation : class, IHandleRequests<TRequest>
        {
            Add(typeof(TRequest), typeof(TImplementation));
        }

        public void Add(Type requestType, Type handlerType)
        {
            var observed = observers.ContainsKey(requestType);
            if (!observed)
            {
                observers.Add(requestType, new List<Type> { handlerType });
            }
            else
            {
                observers[requestType].Add(handlerType);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<Type, List<Type>>> GetEnumerator()
        {
            return observers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
