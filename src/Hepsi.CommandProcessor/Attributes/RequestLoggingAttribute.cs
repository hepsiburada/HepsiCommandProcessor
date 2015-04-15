using System;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.Attributes
{
    /// <summary>
    /// Class RequestLoggingAttribute.
    /// Provides logging of a request using the <see cref="RequestLoggingHandler{T}"/>
    /// </summary>
    public class RequestLoggingAttribute : RequestHandlerAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandlerAttribute" /> class.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <param name="timing">The timing.</param>
        public RequestLoggingAttribute(int step, HandlerTiming timing)
            : base(step, timing)
        { }

        /// <summary>
        /// Initializers the parameters.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public override object[] InitializerParams()
        {
            return new object[] { Timing };
        }

        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        /// <returns>Type.</returns>
        public override Type GetHandlerType()
        {
            return typeof(RequestLoggingHandler<>);
        }
    }
}
