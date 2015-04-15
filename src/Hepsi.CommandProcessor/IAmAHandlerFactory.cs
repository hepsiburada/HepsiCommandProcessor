using System;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAHandlerFactory
    /// We do not know how to create instances of <see cref="IHandleRequests"/> implemented by your application, but need to create instances to instantiate a pipeline.
    /// To achieve this we require clients of the Paramore.Brighter.CommandProcessor library need to implement <see cref="IAmAHandlerFactory"/> to provide 
    /// instances of their <see cref="IHandleRequests"/> types. You need to provide a Handler Factory to support all <see cref="IHandleRequests"/> registered 
    /// with <see cref="IAmASubscriberRegistry"/>. Typically you would use an IoC container to implement the Handler Factory.
    /// </summary>
    public interface IAmAHandlerFactory
    {
        /// <summary>
        /// Creates the specified handler type.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <returns>IHandleRequests.</returns>
        IHandleRequests Create(Type handlerType);
        /// <summary>
        /// Releases the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        void Release(IHandleRequests handler);
    }
}
