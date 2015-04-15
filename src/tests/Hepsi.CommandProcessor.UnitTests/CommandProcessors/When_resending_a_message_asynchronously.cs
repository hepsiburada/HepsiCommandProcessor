using System;
using System.Threading.Tasks;
using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.Messaging;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Polly;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_resending_a_message_asynchronously
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();
        static Message message;
        static Mock<IAmAMessageStore<Message>> commandRepository;
        static Mock<IAmAClientRequestHandler> messagingGateway;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            testCommand.Value = "Hello World";
            commandRepository = new Mock<IAmAMessageStore<Message>>();
            messagingGateway = new Mock<IAmAClientRequestHandler>();
            message = new Message(
                header: new MessageHeader(messageId: testCommand.Id, topic: "TestCommand", messageType: MessageType.MT_COMMAND),
                body: new MessageBody(JsonConvert.SerializeObject(testCommand))
                );

            var retryPolicy = Policy
                .Handle<Exception>()
                .Retry();

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMilliseconds(1));

            commandRepository.Setup(c => c.Get(message.Header.Id)).Returns(Task.FromResult(message));

            commandProcessor = new CommandProcessor(
                new InMemoryRequestContextFactory(),
                new PolicyRegistry { { CommandProcessor.RETRYPOLICY, retryPolicy }, { CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy } },
                new MessageMapperRegistry(new TinyIoCMessageMapperFactory(new TinyIoCContainer())),
                commandRepository.Object,
                messagingGateway.Object,
                logger);

            commandProcessor.Repost(message.Header.Id);
        }

        [Test]
        public void it_should_send_a_message_via_the_messaging_gateway()
        {
            messagingGateway.Verify(m => m.Send(message));
        }
    }
}
