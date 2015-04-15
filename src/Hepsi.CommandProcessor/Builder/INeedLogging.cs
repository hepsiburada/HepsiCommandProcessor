using Common.Logging;


namespace Hepsi.CommandProcessor.Builder
{
    /// <summary>
    /// Interface INeedLogging{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public interface INeedLogging
    {
        /// <summary>
        /// Loggers the specified logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <returns>INeedMessaging.</returns>
        INeedMessaging Logger(ILog logger);
    }
}
