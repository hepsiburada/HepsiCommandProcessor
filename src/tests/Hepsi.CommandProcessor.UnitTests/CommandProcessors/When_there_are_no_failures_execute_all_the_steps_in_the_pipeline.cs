using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_there_are_no_failures_execute_all_the_steps_in_the_pipeline
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestPreAndPostDecoratedHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestPreAndPostDecoratedHandler>();
            container.Register<IHandleRequests<TestCommand>, TestValidationHandler<TestCommand>>();
            container.Register<IHandleRequests<TestCommand>, TestLoggingHandler<TestCommand>>();
            container.Register(logger);
            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);

            commandProcessor.Send(testCommand);
        }

        [Test]
        public void it_should_call_the_pre_validation_handler()
        {
            TestValidationHandler<TestCommand>.ShouldRecieve(testCommand).Should().BeTrue();
        }

        [Test]
        public void it_should_send_the_command_to_the_command_handler()
        {
            TestPreAndPostDecoratedHandler.ShouldRecieve(testCommand).Should().BeTrue();
        }

        [Test]
        public void it_should_call_the_post_validation_handler()
        {
            TestLoggingHandler<TestCommand>.ShouldRecieve(testCommand).Should().BeTrue();
        }
    }
}
