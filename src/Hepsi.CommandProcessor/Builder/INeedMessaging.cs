using Hepsi.CommandProcessor.Messaging;


namespace Hepsi.CommandProcessor.Builder
{
    /// <summary>
    /// Interface INeedMessaging{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public interface INeedMessaging
    {
        /// <summary>
        /// Tasks the queues.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>INeedARequestContext.</returns>
        INeedARequestContext TaskQueues(MessagingConfiguration configuration);
        /// <summary>
        /// Noes the task queues.
        /// </summary>
        /// <returns>INeedARequestContext.</returns>
        INeedARequestContext NoTaskQueues();
    }
}
