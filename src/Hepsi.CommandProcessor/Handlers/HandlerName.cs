namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Class HandlerName
    /// Strongly typed class for the name of a handler
    /// </summary>
    public class HandlerName
    {
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public HandlerName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return name;
        }
    }
}
