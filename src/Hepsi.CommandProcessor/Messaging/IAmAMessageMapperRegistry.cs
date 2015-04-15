namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Interface IAmAMessageMapperRegistry
    /// In order to use a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a> approach we require you to provide
    /// a <see cref="IAmAMessageMapper"/> to map between <see cref="Command"/> or <see cref="Event"/> and a <see cref="Message"/> 
    /// registered via <see cref="IAmAMessageMapperRegistry"/>
    /// The default implementation<see cref="MessageMapperRegistry"/> is suitable for most purposes and the interface is provided for testing
    /// </summary>
    public interface IAmAMessageMapperRegistry
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IAmAMessageMapper&lt;T&gt;.</returns>
        IAmAMessageMapper<T> Get<T>() where T : class, IRequest;
        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TRequest">The type of the t request.</typeparam>
        /// <typeparam name="TMessageMapper">The type of the t message mapper.</typeparam>
        void Register<TRequest, TMessageMapper>()
            where TRequest : class, IRequest
            where TMessageMapper : class, IAmAMessageMapper<TRequest>;
    }
}
