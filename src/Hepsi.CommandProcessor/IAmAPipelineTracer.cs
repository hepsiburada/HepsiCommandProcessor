using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAPipelineTracer
    /// It can be useful to understand what <see cref="IHandleRequests"/> will be called to satisfy a request and their order
    /// The default implementation of <see cref="PipelineTracer"/> can be used in most instances
    /// </summary>
    public interface IAmAPipelineTracer
    {
        /// <summary>
        /// Adds to path.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        void AddToPath(HandlerName handlerName);
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString();
    }
}
