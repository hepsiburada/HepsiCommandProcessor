using System;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.Attributes
{
    /// <summary>
    /// Class TimeoutPolicyAttribute.
    /// This attribute supports setting a timeout on a handler. It is intended for legacy scenarios where the network calls or resource pools used by a handler
    /// do not natively support a timeout and can be used to prevent a handler from blocking on one of these operations. You should not use it where native
    /// timeouts are available, use the native timeout instead.
    /// </summary>
    public class TimeoutPolicyAttribute : RequestHandlerAttribute
    {
        private readonly int milliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutPolicyAttribute"/> class.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <param name="step">The step.</param>
        /// <param name="timing">The timing.</param>
        public TimeoutPolicyAttribute(int milliseconds, int step, HandlerTiming timing = HandlerTiming.Before)
            : base(step, timing)
        {
            this.milliseconds = milliseconds;
        }

        /// <summary>
        /// Initializers the parameters.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public override object[] InitializerParams()
        {
            return new object[] { milliseconds };
        }

        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        /// <returns>Type.</returns>
        public override Type GetHandlerType()
        {
            return typeof(TimeoutPolicyHandler<>);
        }
    }
}
