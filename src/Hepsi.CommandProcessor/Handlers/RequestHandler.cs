using System;
using System.Linq;
using System.Reflection;
using Common.Logging;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Class RequestHandler.
    /// A target of the <see cref="CommandProcessor"/> either as the target of the Command Dispatcher to provide the domain logic required to handle the <see cref="Command"/>
    /// or <see cref="Event"/> or as an orthogonal handler used as part of the Command Processor pipeline.
    /// We recommend deriving your concrete handler from <see cref="RequestHandler{T}"/> instead of implementing the interface as it provides boilerplate
    /// code for calling the next handler in sequence in the pipeline and describing the path
    /// By default the <see cref="Name"/> is based of the Type name, and the <see cref="DescribePath"/> adds that <see cref="Name"/> into the <see cref="IAmAPipelineTracer"/> list.
    /// By default the <see cref="Handle"/> method will log the calls and forward the call to the handler's <see cref="Successor"/>. You should call 
    /// <code>
    /// base.Handle(command); 
    /// </code>
    /// within your derived class handler to forward the call to the next handler in the chain.
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    public abstract class RequestHandler<TRequest> : IHandleRequests<TRequest> where TRequest : class, IRequest
    {
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILog logger;
        private IHandleRequests<TRequest> successor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandler{TRequest}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected RequestHandler(ILog logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public IRequestContext Context { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public HandlerName Name
        {
            get { return new HandlerName(GetType().Name); }
        }

        /// <summary>
        /// Sets the successor.
        /// </summary>
        /// <value>The successor.</value>
        public IHandleRequests<TRequest> Successor
        {
            set { successor = value; }
        }

        /// <summary>
        /// Adds to lifetime.
        /// </summary>
        /// <param name="instanceScope">The instance scope.</param>
        public void AddToLifetime(IAmALifetime instanceScope)
        {
            if (this is IDisposable)
            {
                instanceScope.Add(this);
            }

            if (successor != null)
            {
                successor.AddToLifetime(instanceScope);
            }
        }

        /// <summary>
        /// Describes the path.
        /// </summary>
        /// <param name="pathExplorer">The path explorer.</param>
        public void DescribePath(IAmAPipelineTracer pathExplorer)
        {
            pathExplorer.AddToPath(Name);

            if (successor != null)
            {
                successor.DescribePath(pathExplorer);
            }
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>TRequest.</returns>
        public virtual TRequest Handle(TRequest command)
        {
            if (successor == null)
            {
                return command;
            }

            logger.Debug(m => m("Passing request from {0} to {1}", Name, successor.Name));

            return successor.Handle(command);
        }

        //default is just to do nothing - use this if you need to pass data from an attribute into a handler
        /// <summary>
        /// Initializes from attribute parameters.
        /// </summary>
        /// <param name="initializerList">The initializer list.</param>
        public virtual void InitializeFromAttributeParams(params object[] initializerList) { }


        internal MethodInfo FindHandlerMethod()
        {
            var methods = GetType().GetMethods();

            return methods
                .Where(method => method.Name == "Handle")
                .SingleOrDefault(method => method.GetParameters().Count() == 1 && method.GetParameters().Single().ParameterType == typeof(TRequest));
        }
    }
}
