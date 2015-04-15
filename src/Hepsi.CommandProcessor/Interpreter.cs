using System;
using System.Collections.Generic;
using System.Linq;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class Interpreter
    /// The <see cref="Interpreter{T}"/> is the dispatcher element of the Command Dispatcher. It looks up the <see cref="IRequest"/> in the <see cref="SubscriberRegistry"/>
    /// to find registered <see cref="IHandleRequests"/> and returns to the PipelineBuilder, which in turn will call the client provided <see cref="IAmAHandlerFactory"/>
    /// to create instances of the the handlers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    internal class Interpreter<TRequest> where TRequest : class, IRequest
    {
        private readonly IAmASubscriberRegistry registry;
        private readonly IAmAHandlerFactory handlerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpreter{TRequest}"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        internal Interpreter(IAmASubscriberRegistry registry, IAmAHandlerFactory handlerFactory)
        {
            this.registry = registry;
            this.handlerFactory = handlerFactory;
        }

        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <param name="requestType">Type of the request.</param>
        /// <returns>IEnumerable&lt;RequestHandler&lt;TRequest&gt;&gt;.</returns>
        internal IEnumerable<RequestHandler<TRequest>> GetHandlers(Type requestType)
        {
            return new RequestHandlers<TRequest>(registry.Get<TRequest>().Select(handlerType => handlerFactory.Create(handlerType)).Cast<IHandleRequests<TRequest>>());
        }
    }
}
