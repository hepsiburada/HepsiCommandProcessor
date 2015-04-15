using System.Collections.Generic;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Any pipeline has a request context that allows you to flow information between instances of <see cref="IHandleRequests"/>
    /// The default in-memory <see cref="RequestContext"/> created by an <see cref="InMemoryRequestContextFactory"/> is suitable for most purposes
    /// and this interface is mainly provided for testing
    /// </summary>
    public interface IRequestContext
    {
        /// <summary>
        /// Gets the bag.
        /// </summary>
        /// <value>The bag.</value>
        Dictionary<string, object> Bag { get; }
        /// <summary>
        /// Gets the policies.
        /// </summary>
        /// <value>The policies.</value>
        IAmAPolicyRegistry Policies { get; }
    }
}
