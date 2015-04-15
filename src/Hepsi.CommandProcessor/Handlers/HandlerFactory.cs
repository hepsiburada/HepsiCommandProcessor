using System;
using Hepsi.CommandProcessor.Attributes;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Class HandlerFactory
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    internal class HandlerFactory<TRequest> where TRequest : class, IRequest
    {
        private readonly RequestHandlerAttribute attribute;
        private readonly IAmAHandlerFactory factory;
        private readonly Type messageType;
        private readonly IRequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerFactory{TRequest}"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="requestContext">The request context.</param>
        public HandlerFactory(RequestHandlerAttribute attribute, IAmAHandlerFactory factory, IRequestContext requestContext)
        {
            this.attribute = attribute;
            this.factory = factory;
            this.requestContext = requestContext;
            messageType = typeof(TRequest);
        }

        /// <summary>
        /// Creates the request handler.
        /// </summary>
        /// <returns>IHandleRequests&lt;TRequest&gt;.</returns>
        public IHandleRequests<TRequest> CreateRequestHandler()
        {
            var handlerType = attribute.GetHandlerType().MakeGenericType(messageType);
            var handler = (IHandleRequests<TRequest>)factory.Create(handlerType);

            //Load the context befor the initializer - in case we want to use the context from within the initializer
            handler.Context = requestContext;
            handler.InitializeFromAttributeParams(attribute.InitializerParams());

            return handler;
        }
    }
}
