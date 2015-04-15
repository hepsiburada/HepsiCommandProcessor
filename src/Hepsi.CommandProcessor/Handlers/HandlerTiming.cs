using Hepsi.CommandProcessor.Attributes;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Enum HandlerTiming
    /// The Subscriber Registry registers 'target' handlers for commands or events.
    /// A <see cref="RequestHandlerAttribute"/> identifies handlers to run in the pipeline with the target handler, to deal with 'orthogonal' concerns
    /// Those handlers can run either before or after the target handler, and the timing value indicates where they should run
    /// Note that handlers explicitly call the next handler in sequence, so 'child' handlers always run in the scope of their 'parent' handlers, which means
    /// that you can choose to only execute code in a 'parent' only after a 'child' handler has executed. So you can control order of operation by that approach
    /// and do not need to use an After handler for that.
    /// </summary>
    public enum HandlerTiming
    {
        /// <summary>
        /// Execute this 'orthogonal' handler before the 'target' handler
        /// </summary>
        Before = 0,
        /// <summary>
        /// Execute this 'orthogonal' handler after the 'target' handler 
        /// </summary>
        After = 1
    }
}
