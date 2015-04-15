using System.Collections.Generic;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class RequestContext
    /// Any pipeline has a request context that allows you to flow information between instances of <see cref="IHandleRequests"/>
    /// The default in-memory <see cref="RequestContext"/> created by an <see cref="InMemoryRequestContextFactory"/> is suitable for most purposes
    /// and this interface is mainly provided for testing
    /// </summary>
    public class RequestContext : IRequestContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestContext"/> class.
        /// </summary>
        public RequestContext()
        {
            Bag = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the bag.
        /// </summary>
        /// <value>The bag.</value>
        public Dictionary<string, object> Bag { get; private set; }
        /// <summary>
        /// Gets the policies.
        /// </summary>
        /// <value>The policies.</value>
        public IAmAPolicyRegistry Policies { get; set; }
    }
}
