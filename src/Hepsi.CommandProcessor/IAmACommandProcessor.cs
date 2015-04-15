using System;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmACommandProcessor
    /// Paramore.Brighter.CommandProcessor provides the default implementation of this interface <see cref="CommandProcessor"/> and it is unlikely you need
    /// to override this for anything other than testing purposes. The usual need is that in a <see cref="RequestHandler{TRequest}"/> you intend to publish an  
    /// <see cref="Event"/> to indicate the handler has completed to other components. In this case your tests should only verify that the correct 
    /// event was raised by listening to <see cref="Publish{T}"/> calls on this interface, using a mocking framework of your choice or bespoke
    /// Test Double.
    /// </summary>
    public interface IAmACommandProcessor
    {
        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        void Send<T>(T command) where T : class, IRequest;
        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event">The event.</param>
        void Publish<T>(T @event) where T : class, IRequest;
        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        void Post<T>(T request) where T : class, IRequest;
        /// <summary>
        /// Reposts the specified message identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        void Repost(Guid messageId);
    }
}
