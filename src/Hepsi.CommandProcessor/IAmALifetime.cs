using System;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmALifetime
    /// Used to manage the lifetime of objects created for the request handling pipeline
    /// <see cref="LifetimeScope"/> for default implementation.
    /// </summary>
    public interface IAmALifetime : IDisposable
    {
        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Add(IHandleRequests instance);
    }
}
