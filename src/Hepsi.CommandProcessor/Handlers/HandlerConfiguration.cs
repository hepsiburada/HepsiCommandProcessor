namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Class HandlerConfiguration
    /// </summary>
    public class HandlerConfiguration
    {
        /// <summary>
        /// Gets the subscriber registry.
        /// </summary>
        /// <value>The subscriber registry.</value>
        public IAmASubscriberRegistry SubscriberRegistry { get; private set; }
        /// <summary>
        /// Gets the handler factory.
        /// </summary>
        /// <value>The handler factory.</value>
        public IAmAHandlerFactory HandlerFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerConfiguration"/> class.
        /// We use the <see cref="IAmASubscriberRegistry"/> instance to look up subscribers for messages when dispatching. Use <see cref="SubscriberRegistry"/> unless
        /// you have some reason to override. We expect a <see cref="CommandProcessor.Send{T}(T)"/> to have one registered handler
        /// We use the 
        /// </summary>
        /// <param name="subscriberRegistry">The subscriber registry.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        public HandlerConfiguration(IAmASubscriberRegistry subscriberRegistry, IAmAHandlerFactory handlerFactory)
        {
            SubscriberRegistry = subscriberRegistry;
            HandlerFactory = handlerFactory;
        }
    }
}
