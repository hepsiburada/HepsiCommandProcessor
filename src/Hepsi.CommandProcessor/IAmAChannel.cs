namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAChannel
    /// A channel is an abstraction for an <a href="http://en.wikipedia.org/wiki/OSI_model">OSI model</a> Application Layer used to provide support for a 
    /// <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a> pattern of dispatch and processing
    /// </summary>
    public interface IAmAChannel
    {
        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        int Length { get; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        ChannelName Name { get; }
    }
}
