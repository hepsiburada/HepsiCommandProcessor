using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.Builder
{
    /// <summary>
    /// Interface INeedAHandlers{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public interface INeedAHandlers
    {
        /// <summary>
        /// Handlerses the specified the registry.
        /// </summary>
        /// <param name="theRegistry">The registry.</param>
        /// <returns>INeedPolicy.</returns>
        INeedPolicy Handlers(HandlerConfiguration theRegistry);
    }
}
