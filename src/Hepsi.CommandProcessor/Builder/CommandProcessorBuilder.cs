using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.Messaging;
using Polly;

namespace Hepsi.CommandProcessor.Builder
{
    /// <summary>
    /// Class CommandProcessorBuilder.{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// Provides a fluent interface to construct a <see cref="CommandProcessor"/>. We need to identify the following dependencies in order to create a <see cref="CommandProcessor"/>
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             A <see cref="HandlerConfiguration"/> containing a <see cref="IAmASubscriberRegistry"/> and a <see cref="IAmAHandlerFactory"/>. You can use <see cref="SubscriberRegistry"/>
    ///             to provide the <see cref="IAmASubscriberRegistry"/> but you need to implement your own  <see cref="IAmAHandlerFactory"/>, for example using your preferred Inversion of Control
    ///             (IoC) container
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             A <see cref="IAmAPolicyRegistry"/> containing a list of policies that you want to be accessible to the <see cref="CommandProcessor"/>. You can use
    ///             <see cref="PolicyRegistry"/> to provide the <see cref="IAmAPolicyRegistry"/>. Policies are expected to be Polly <see cref="!:https://github.com/michael-wolfenden/Polly"/> 
    ///             <see cref="Policy"/> references.
    ///             If you do not need any policies around quality of service (QoS) concerns - you do not have Work Queues and/or do not intend to use Polly Policies for 
    ///             QoS concerns - you can use <see cref="NoPolicy"/> to indicate you do not need them.
    ///         </description>
    ///      </item>
    ///     <item>
    ///         <description>
    ///             A <see cref="ILog"/> that is the logger to use for diagnostic feedback. <see cref="ILog"/> is defined in 
    ///             Common.Logging <see cref="!:https://github.com/net-commons/common-logging"/> as an abstraction over logging frameworks and allows us to support your
    ///             preferred logging framework
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             A <see cref="MessagingConfiguration"/> describing how you want to configure Task Queues for the <see cref="CommandProcessor"/>. We store messages in a <see cref="IAmAMessageStore"/>
    ///             for later replay (in case we need to compensate by trying a message again). We send messages to a Task Queue via a <see cref="IAmAClientRequestHandler"/> and we  want to know how
    ///             to map the <see cref="IRequest"/> (<see cref="Command"/> or <see cref="Event"/>) to a <see cref="Message"/> using a <see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapper"/> using 
    ///             an <see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapperRegistry"/>. You can use the default <see cref="MessageMapperRegistry"/> to register the association. You need to 
    ///             provide a <see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapperFactory"/> so that we can create instances of your  <see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapper"/>. You need to provide a <see cref="Hepsi.CommandProcessor.Messaging.IAmAMessageMapperFactory"/>
    ///             when using <see cref="MessageMapperRegistry"/> so that we can create instances of your mapper. 
    ///             If you don't want to use Task Queues i.e. you are just using a synchronous Command Dispatcher approach, then use the <see cref="NoTaskQueues"/> method to indicate your intent
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             Finally we need to provide a <see cref="IRequestContext"/> to provide context to requests handlers in the pipeline that can be used to pass information without using the message
    ///             that initiated the pipeline. We instantiate this via a user-provided <see cref="IAmARequestContextFactory"/>. The default approach is use <see cref="InMemoryRequestContextFactory"/>
    ///             to provide a <see cref="RequestContext"/> unless you have a requirement to replace this, such as in testing.
    ///         </description>
    ///     </item>
    /// </list> 
    /// </summary>
    public class CommandProcessorBuilder : INeedAHandlers, INeedPolicy, INeedLogging, INeedMessaging, INeedARequestContext, IAmACommandProcessorBuilder
    {
        private ILog logger;
        private IAmAMessageStore<Message> messageStore;
        private IAmAClientRequestHandler messagingGateway;
        private IAmAMessageMapperRegistry messageMapperRegistry;
        private IAmARequestContextFactory requestContextFactory;
        private IAmASubscriberRegistry registry;
        private IAmAHandlerFactory handlerFactory;
        private IAmAPolicyRegistry policyRegistry;
        private CommandProcessorBuilder() { }

        /// <summary>
        /// Begins the Fluent Interface
        /// </summary>
        /// <returns>INeedAHandlers.</returns>
        public static INeedAHandlers With()
        {
            return new CommandProcessorBuilder();
        }

        /// <summary>
        /// Supplies the specified handler configuration, so that we can register subscribers and the handler factory used to create instances of them
        /// </summary>
        /// <param name="handlerConfiguration">The handler configuration.</param>
        /// <returns>INeedPolicy.</returns>
        public INeedPolicy Handlers(HandlerConfiguration handlerConfiguration)
        {
            registry = handlerConfiguration.SubscriberRegistry;
            handlerFactory = handlerConfiguration.HandlerFactory;
            return this;
        }

        /// <summary>
        /// Supplies the specified the policy registry, so we can use policies for Task Queues or in user-defined request handlers such as ExceptionHandler
        /// that provide quality of service concerns
        /// </summary>
        /// <param name="thePolicyRegistry">The policy registry.</param>
        /// <returns>INeedLogging.</returns>
        public INeedLogging Policies(IAmAPolicyRegistry thePolicyRegistry)
        {
            policyRegistry = thePolicyRegistry;
            return this;
        }

        /// <summary>
        /// Use this if you do not require policy (i.e. No Tasks Queues or QoS needs).
        /// </summary>
        /// <returns>INeedLogging.</returns>
        public INeedLogging NoPolicy()
        {
            return this;
        }
        /// <summary>
        /// Use the specified logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <returns>INeedMessaging.</returns>
        public INeedMessaging Logger(ILog logger)
        {
            this.logger = logger;

            return this;
        }

        /// <summary>
        /// The <see cref="CommandProcessor"/> wants to support <see cref="CommandProcessor.Post{T}(T)"/> or <see cref="CommandProcessor.Repost"/> using Task Queues.
        /// You need to provide a policy to specify how QoS issues, specifically <see cref="CommandProcessor.RETRYPOLICY "/> or <see cref="CommandProcessor.CIRCUITBREAKER "/> 
        /// are handled by adding appropriate <see cref="Policies"/> when choosing this option.
        /// 
        /// </summary>
        /// <param name="configuration">The Task Queues configuration.</param>
        /// <returns>INeedARequestContext.</returns>
        public INeedARequestContext TaskQueues(MessagingConfiguration configuration)
        {
            messageStore = configuration.MessageStore;
            messagingGateway = configuration.MessagingGateway;
            messageMapperRegistry = configuration.MessageMapperRegistry;
            return this;
        }

        /// <summary>
        /// Use to indicate that you are not using Task Queues.
        /// </summary>
        /// <returns>INeedARequestContext.</returns>
        public INeedARequestContext NoTaskQueues()
        {
            return this;
        }

        /// <summary>
        /// The factory for <see cref="IRequestContext"/> used within the pipeline to pass information between <see cref="IHandleRequests{T}"/> steps. If you do not need to override
        /// provide <see cref="InMemoryRequestContextFactory"/>.
        /// </summary>
        /// <param name="requestContextFactory">The request context factory.</param>
        /// <returns>IAmACommandProcessorBuilder.</returns>
        public IAmACommandProcessorBuilder RequestContextFactory(IAmARequestContextFactory requestContextFactory)
        {
            this.requestContextFactory = requestContextFactory;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="CommandProcessor"/> from the configuration.
        /// </summary>
        /// <returns>CommandProcessor.</returns>
        public CommandProcessor Build()
        {
            return new CommandProcessor(
                subscriberRegistry: registry,
                handlerFactory: handlerFactory,
                requestContextFactory: requestContextFactory,
                policyRegistry: policyRegistry,
                mapperRegistry: messageMapperRegistry,
                messageStore: messageStore,
                messagingGateway: messagingGateway,
                logger: logger
                );
        }
    }
}
