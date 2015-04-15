namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmARequestContextFactory
    /// Any pipeline has a request context that allows you to flow information between instances of <see cref="IHandleRequests"/>
    /// The default <see cref="InMemoryRequestContextFactory"/> is usable for most cases, and this interface mainly supports testing
    /// </summary>
    public interface IAmARequestContextFactory
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>RequestContext.</returns>
        RequestContext Create();
    }
}
