namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Interface IAmAMessageMapper
    /// Map between a <see cref="Command"/> or an <see cref="Event"/> and a <see cref="Message"/>. You must implement this for each Command or Message you intend to send over
    /// a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a> 
    /// </summary>
    public interface IAmAMessageMapper { }

    /// <summary>
    /// Interface IAmAMessageMapper
    /// In order to use a Task Queue
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    public interface IAmAMessageMapper<TRequest> : IAmAMessageMapper where TRequest : class, IRequest
    {
        /// <summary>
        /// Maps to message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Message.</returns>
        Message MapToMessage(TRequest request);
        /// <summary>
        /// Maps to request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>TRequest.</returns>
        TRequest MapToRequest(Message message);
    }
}
