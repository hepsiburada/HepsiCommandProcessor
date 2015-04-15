using System;


namespace Hepsi.CommandProcessor.Messaging
{
    /// <summary>
    /// Class MessageFactory.{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public static class MessageFactory
    {
        /// <summary>
        /// Creates the empty message.
        /// </summary>
        /// <returns>Message.</returns>
        public static Message CreateEmptyMessage()
        {
            return new Message();
        }

        /// <summary>
        /// Creates the quit message.
        /// </summary>
        /// <returns>Message.</returns>
        public static Message CreateQuitMessage()
        {
            return new Message(new MessageHeader(Guid.Empty, string.Empty, MessageType.MT_QUIT), new MessageBody(string.Empty));
        }
    }
}
