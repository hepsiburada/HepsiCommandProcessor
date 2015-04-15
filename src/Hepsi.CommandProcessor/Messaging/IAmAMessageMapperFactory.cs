using System;

namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Interface IAmAMessageMapperFactory
    /// In order to use a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a> approach we require you to provide
    /// a <see cref="IAmAMessageMapper"/> to map between <see cref="Command"/> or <see cref="Event"/> and a <see cref="Message"/> registered via <see cref="IAmAMessageMapperRegistry"/>
    /// We then call the instance of the factory which the client provides to create instances of that <see cref="IAmAMessageMapper"/>. You will need to implement the
    /// <see cref="IAmAMessageMapperFactory"/> to use the Task Queue approach, and provide the instance of your mapper on request. Typically you might use an IoC container
    /// to implement this.
    /// </summary>
    public interface IAmAMessageMapperFactory
    {
        /// <summary>
        /// Creates the specified message mapper type.
        /// </summary>
        /// <param name="messageMapperType">Type of the message mapper.</param>
        /// <returns>IAmAMessageMapper.</returns>
        IAmAMessageMapper Create(Type messageMapperType);
    }
}
