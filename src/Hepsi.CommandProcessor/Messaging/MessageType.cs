namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Enum MessageType
    /// The type of a message, used on the receiving side of a Task Queue to handle the message appropriately
    /// </summary>
    public enum MessageType
    {
        MT_UNACCEPTABLE = -1,
        MT_NONE = 0,
        /// <summary>
        /// The m t_ command{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        MT_COMMAND = 1,
        /// <summary>
        /// The m t_ event{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        MT_EVENT = 2,
        /// <summary>
        /// The m t_ document{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        MT_DOCUMENT = 3,
        /// <summary>
        /// The m t_ quit{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        MT_QUIT = 4
    }
}
