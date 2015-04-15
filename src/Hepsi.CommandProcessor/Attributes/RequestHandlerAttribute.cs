using System;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.Attributes
{
    /// <summary>
    /// Class RequestHandlerAttribute.
    /// To satisfy orthogonal concerns it is possible to create a pipeline of <see cref="IHandleRequests"/> handlers. The 'target' handler should handle the domain
    /// logic, the other handlers in the pipeline should handle Quality of Service concerns or similar orthogonal concerns. We use an approach of attributing the <see cref="IHandleRequests{T}.Handle"/>
    /// method to indicate the other handlers in the pipeline that handle orthogonal concerns. This approach is preferred over fluent-pipeline configuration
    /// because it allows you to easily see orthogonal concerns within the context of the target handler. In this sense Brighter is 'opinionated' about approach.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class RequestHandlerAttribute : Attribute
    {
        private readonly int step;

        private readonly HandlerTiming timing;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandlerAttribute"/> class.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <param name="timing">The timing.</param>
        protected RequestHandlerAttribute(int step, HandlerTiming timing = HandlerTiming.Before)
        {
            this.step = step;
            this.timing = timing;
        }

        //We use this to pass params from the attribute into the instance of the handler
        //if you need to pass additional params to your handler, use this
        /// <summary>
        /// Initializers the parameters.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public virtual object[] InitializerParams()
        {
            return new object[0];
        }

        //In which order should we run this, within the pre or post sequence for the main target?
        /// <summary>
        /// Gets the step.
        /// </summary>
        /// <value>The step.</value>
        public int Step
        {
            get { return step; }
        }

        //Should we run this before or after the main target?
        /// <summary>
        /// Gets the timing.
        /// </summary>
        /// <value>The timing.</value>
        public HandlerTiming Timing
        {
            get { return timing; }
        }

        //What type do we implement for the Filter in the Command Processor Pipeline
        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        /// <returns>Type.</returns>
        public abstract Type GetHandlerType();
    }
}
