using System;
using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.Messaging;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Polly;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors.When_using_decoupled_invocation
{
    [TestFixture]
    public class to_send_a_message_asynchronously
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

            var messageMapperRegistry = new MessageMapperRegistry(new TestMessageMapperFactory(() => new TestCommandMessageMapper()));
            messageMapperRegistry.Register<TestCommand, TestCommandMessageMapper>();

            var retryPolicy = Policy
                .Handle<Exception>()
                .Retry();

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMilliseconds(1));

            commandProcessor = new CommandProcessor(
                new InMemoryRequestContextFactory(),
                new PolicyRegistry() { { CommandProcessor.RETRYPOLICY, retryPolicy }, { CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy } },
                messageMapperRegistry,
                commandRepository.Object,
                messagingGateway.Object,
                logger);

            commandProcessor.Post(testCommand);
        }

        [Test]
        public void it_should_store_the_message_in_the_sent_command_message_repository()
        {
            commandRepository.Object.Add(message);
        }

        [Test]
        public void it_should_send_a_message_via_the_messaging_gateway()
        {
            messagingGateway.Verify(m => m.Send(message));
        }
    }
}
