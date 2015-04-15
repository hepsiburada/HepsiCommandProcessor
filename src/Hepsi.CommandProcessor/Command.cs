using System;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class Command. 
    /// A command is an imperative instruction to do something. We expect only one receiver of a command because it is point-to-point
    /// </summary>
    [Serializable]
    public class Command : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Command(Guid id)
        {
            Id = id;
        }
    }
}
