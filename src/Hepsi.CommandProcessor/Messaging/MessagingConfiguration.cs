using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Class MessagingConfiguration.{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public class MessagingConfiguration
    {
        /// <summary>
        /// Gets the message store.
        /// </summary>
        /// <value>The message store.</value>
        public IAmAMessageStore<Message> MessageStore { get; private set; }
        /// <summary>
        /// Gets the messaging gateway.
        /// </summary>
        /// <value>The messaging gateway.</value>
        public IAmAClientRequestHandler MessagingGateway { get; private set; }
        /// <summary>
        /// Gets the message mapper registry.
        /// </summary>
        /// <value>The message mapper registry.</value>
        public IAmAMessageMapperRegistry MessageMapperRegistry { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingConfiguration"/> class.
        /// </summary>
        /// <param name="messageStore">The message store.</param>
        /// <param name="messagingGateway">The messaging gateway.</param>
        /// <param name="messageMapperRegistry">The message mapper registry.</param>
        public MessagingConfiguration(
            IAmAMessageStore<Message> messageStore,
            IAmAClientRequestHandler messagingGateway,
            IAmAMessageMapperRegistry messageMapperRegistry
            )
        {
            MessageStore = messageStore;
            MessagingGateway = messagingGateway;
            MessageMapperRegistry = messageMapperRegistry;
        }
    }
}
