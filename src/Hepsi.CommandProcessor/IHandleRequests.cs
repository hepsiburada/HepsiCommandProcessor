using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IHandleRequests
    /// A target of the <see cref="CommandProcessor"/> either as the target of the Command Dispatcher to provide the domain logic required to handle the <see cref="Command"/>
    /// or <see cref="Event"/> or as an orthogonal handler used as part of the Command Processor pipeline.
    /// We recommend deriving your concrete handler from <see cref="RequestHandler{T}"/> instead of implementing the interface as it provides boilerplate
    /// code for calling the next handler in sequence in the pipeline and describing the path
    /// The <see cref="IHandleRequests"/> interface contains a contract not dependant on the <see cref="IRequest"/> and is useful when you need to deal with a handler
    /// without knowing the specific <see cref="IRequest"/> type, but most implementations should use <see cref="IHandleRequests{T}"/> directly
    /// </summary>
    public interface IHandleRequests
    {
        /// <summary>
        /// Gets or sets the context. Usually the context is given to you by the pipeline and you do not need to set this
        /// </summary>
        /// <value>The context.</value>
        IRequestContext Context { get; set; }
        /// <summary>
        /// Describes the path. To support pipeline tracing. Generally return the name of this handler to <see cref="IAmAPipelineTracer"/>,
        ///  or other information to determine the path a request will take
        /// </summary>
        /// <param name="pathExplorer">The path explorer.</param>
        void DescribePath(IAmAPipelineTracer pathExplorer);
        /// <summary>
        /// Initializes from the <see cref="RequestHandlerAttribute"/> attribute parameters. Use when you need to provide parameter information from the
        /// attribute to the handler. Note that the attribute implementation might include types other than primitives that you intend to pass across, but
        /// the attribute itself can only use primitives.
        /// You couple the handler to a specific attribute using this as you need to know about the parameters passed, so this is really only appropriate
        /// for an attribute-handler pair used to provide orthogonal QoS to the pipeline.
        /// </summary>
        /// <param name="initializerList">The initializer list.</param>
        void InitializeFromAttributeParams(params object[] initializerList);
        /// <summary>
        /// Gets the name of the Handler. Useful for diagnostic purposes
        /// </summary>
        /// <value>The name.</value>
        HandlerName Name { get; }
    }

    /// <summary>
    /// Interface IHandleRequests
    /// A target of the <see cref="CommandProcessor"/> either as the target of the Command Dispatcher to provide the domain logic required to handle the <see cref="Command"/>
    /// or <see cref="Event"/> or as an orthogonal handler used as part of the Command Processor pipeline.
    /// We recommend deriving your concrete handler from <see cref="RequestHandler{T}"/> instead of implementing the interface as it provides boilerplate
    /// code for calling the next handler in sequence in the pipeline and describing the path.
    /// It derives from <see cref="IHandleRequests"/> which provides functionality that is not dependant on <see cref="IRequest"/>. This simplifies some tasks that do not know
    /// the specific type of the <see cref="IRequest"/>
    /// Impelementors should use on class to implement both <see cref="IHandleRequests{T}"/> and <see cref="IHandleRequests"/> as per <see cref="RequestHandler{T}"/>
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    public interface IHandleRequests<TRequest> : IHandleRequests where TRequest : class, IRequest
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>TRequest.</returns>
        TRequest Handle(TRequest command);
        /// <summary>
        /// Sets the successor.
        /// </summary>
        /// <value>The successor.</value>
        IHandleRequests<TRequest> Successor { set; }
        /// <summary>
        /// Adds to lifetime so that the pipeline can manage destroying handlers created as part of the pipeline by calling the client provided <see cref="IAmAHandlerFactory"/> .
        /// </summary>
        /// <param name="instanceScope">The instance scope.</param>
        void AddToLifetime(IAmALifetime instanceScope);
    }
}
