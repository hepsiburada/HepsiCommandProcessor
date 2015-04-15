using System;
using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.Messaging;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Polly;
using Polly.CircuitBreaker;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors.When_using_decoupled_invocation
{
    [TestFixture]
    public class messaging_gateway_throws_an_error_retry_n_times_then_break_circuit
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

            messagingGateway.Setup(m => m.Send(message)).Throws<Exception>();

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromMilliseconds(50),
                    TimeSpan.FromMilliseconds(100),
                    TimeSpan.FromMilliseconds(150)
                });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMinutes(1));

            commandProcessor = new CommandProcessor(
                 new InMemoryRequestContextFactory(),
                 new PolicyRegistry { { CommandProcessor.RETRYPOLICY, retryPolicy }, { CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy } },
                 messageMapperRegistry,
                 commandRepository.Object,
                 messagingGateway.Object,
                 logger);
        }

        [Test]
        public void it_should_send_a_message_via_the_messaging_gateway()
        {
            Assert.Throws<Exception>(() => commandProcessor.Post(testCommand));

            messagingGateway.Verify(m => m.Send(message), Times.Exactly(4));
        }

        [Test]
        [ExpectedException(typeof(BrokenCircuitException))]
        public void it_should_throw_a_circuit_broken_exception_once_circuit_broken()
        {
            commandProcessor.Post(testCommand);
        }
    }
}
