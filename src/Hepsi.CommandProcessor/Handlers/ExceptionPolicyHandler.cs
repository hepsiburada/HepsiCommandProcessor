using System;
using Common.Logging;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Builder;
using Polly;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Class ExceptionPolicyHandler.
    /// The <see cref="UsePolicyAttribute"/> supports the use of <a href="https://github.com/michael-wolfenden/Polly">Polly</a> to provide quality of service around exceptions
    /// thrown from subsequent steps in the handler pipeline. A Polly Policy can be used to support a Retry or Circuit Breaker approach to exception handling
    /// Policies used by the attribute are identified by a string based key, which is used as a lookup into an <see cref="IAmAPolicyRegistry"/> and it is 
    /// assumed that you have registered required policies with a Policy Registry such as <see cref="PolicyRegistry"/> and configured that as a 
    /// dependency of the <see cref="CommandProcessor"/> using the <see cref="CommandProcessorBuilder"/>
    /// The ExceptionPolicyHandler is instantiated by the pipeline when the <see cref="UsePolicyAttribute"/> is added to the <see cref="IHandleRequests{T}.Handle"/> method
    /// of the target handler implemented by the client.
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    public class ExceptionPolicyHandler<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        private Policy policy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyHandler{TRequest}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ExceptionPolicyHandler(ILog logger)
            : base(logger)
        { }

        /// <summary>
        /// Initializes from attribute parameters. This will get the <see cref="IAmAPolicyRegistry"/> from the <see cref="IRequestContext"/> and query it for the
        /// policy identified in <see cref="UsePolicyAttribute"/>
        /// </summary>
        /// <param name="initializerList">The initializer list.</param>
        /// <exception cref="System.ArgumentException">Could not find the policy for this attribute, did you register it with the command processor's container;initializerList</exception>
        public override void InitializeFromAttributeParams(params object[] initializerList)
        {
            //we expect the first and only parameter to be a string
            var policyName = (string)initializerList[0];
            policy = Context.Policies.Get(policyName);
            if (policy == null)
            {
                throw new ArgumentException("Could not find the policy for this attribute, did you register it with the command processor's container", "initializerList");
            }
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>TRequest.</returns>
        public override TRequest Handle(TRequest command)
        {
            return policy.Execute(() => base.Handle(command));
        }
    }
}
