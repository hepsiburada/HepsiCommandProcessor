namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAChannelFactory
    /// Creates instances of <see cref="IAmAChannel"/>channels. We provide support for some Application Layer channels, and provide factories for those:
    /// <list type="bullet">
    /// <item>AMQP</item>
    /// <item>RestML</item>
    /// </list>
    /// If you need to support other Application Layer protocols, please consider issuing a Pull request for your implementation
    /// </summary>
    public interface IAmAChannelFactory
    {
        /// <summary>
        /// Creates the input channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="routingKey"></param>
        /// <returns>IAmAnInputChannel.</returns>
        IAmAnInputChannel CreateInputChannel(string channelName, string routingKey);

        /// <summary>
        /// Creates the output channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="routingKey"></param>
        /// <returns>IAmAnInputChannel.</returns>
        IAmAnInputChannel CreateOutputChannel(string channelName, string routingKey);
    }
}
