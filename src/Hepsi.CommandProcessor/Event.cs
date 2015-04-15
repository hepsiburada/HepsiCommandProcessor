using System;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class Event
    /// An event is an indicator to interested parties that 'something has happened'. We expect zero to many receivers as it is one-to-many communication i.e. publish-subscribe
    /// An event is usually fire-and-forget, because we do not know it is received.
    /// </summary>
    public class Event : IRequest
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }
    }
}
