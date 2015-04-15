using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_sending_a_command_to_the_processor
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestCommandHandler>();

            var handlerFactory = new TestHandlerFactory<TestCommand, TestCommandHandler>(() => new TestCommandHandler(logger));

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);

            commandProcessor.Send(testCommand);
        }

        [Test]
        public void it_should_send_the_command_to_the_command_handler()
        {
            TestCommandHandler.ShouldRecieve(testCommand).Should().BeTrue();
        }
    }
}
