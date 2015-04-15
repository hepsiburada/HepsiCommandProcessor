using System;
using Hepsi.CommandProcessor.Builder;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.Attributes
{
    /// <summary>
    /// Class UsePolicyAttribute.
    /// This attribute supports the use of <a href="https://github.com/michael-wolfenden/Polly">Polly</a> to provide quality of service around exceptions
    /// thrown from subsequent steps in the handler pipeline. A Polly Policy can be used to support a Retry or Circuit Breaker approach to exception handling
    /// Policies used by the attribute are identified by a string based key, which is used as a lookup into an <see cref="IAmAPolicyRegistry"/> and it is 
    /// assumed that you have registered required policies with a Policy Registry such as <see cref="PolicyRegistry"/> and configured that as a 
    /// dependency of the <see cref="CommandProcessor"/> using the <see cref="CommandProcessorBuilder"/>
    /// </summary>
    public class UsePolicyAttribute : RequestHandlerAttribute
    {
        private readonly string policy;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsePolicyAttribute"/> class.
        /// </summary>
        /// <param name="policy">The policy key, used as a lookup into an <see cref="IAmAPolicyRegistry"/>.</param>
        /// <param name="step">The step.</param>
        public UsePolicyAttribute(string policy, int step)
            : base(step, HandlerTiming.Before)
        {
            this.policy = policy;
        }

        /// <summary>
        /// Initializers the parameters.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public override object[] InitializerParams()
        {
            return new object[] { policy };
        }

        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        /// <returns>Type.</returns>
        public override Type GetHandlerType()
        {
            return typeof(ExceptionPolicyHandler<>);
        }
    }
}
