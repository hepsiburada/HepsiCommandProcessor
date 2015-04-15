namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class InMemoryRequestContextFactory
    /// </summary>
    public class InMemoryRequestContextFactory : IAmARequestContextFactory
    {
        /// <summary>
        /// Any pipeline has a request context that allows you to flow information between instances of <see cref="IHandleRequests"/>
        /// The default <see cref="InMemoryRequestContextFactory"/> is usable for most cases.
        /// </summary>
        /// <returns>RequestContext.</returns>
        public RequestContext Create()
        {
            return new RequestContext();
        }
    }
}
