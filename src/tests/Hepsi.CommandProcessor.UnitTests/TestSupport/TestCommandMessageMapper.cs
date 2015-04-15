using Hepsi.CommandProcessor.Messaging;
using Newtonsoft.Json;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport
{
    internal class TestCommandMessageMapper : IAmAMessageMapper<TestCommand>
    {
        public Message MapToMessage(TestCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "TestCommand", messageType: MessageType.MT_COMMAND);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;
        }

        public TestCommand MapToRequest(Message message)
        {
            var command = JsonConvert.DeserializeObject<TestCommand>(message.Body.Value);
            return command;
        }
    }
}
