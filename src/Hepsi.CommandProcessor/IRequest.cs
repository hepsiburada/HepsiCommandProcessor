using System;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IRequest
    /// Base class of <see cref="Command"/> and <see cref="Event"/>. A request that can be handled by the Command Processor/Dispatcher
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        Guid Id { get; set; }
    }
}
