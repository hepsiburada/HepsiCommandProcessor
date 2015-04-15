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
    public class When_an_exception_is_thrown_terminate_the_pipeline
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestUnusedCommandHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestUnusedCommandHandler>();
            container.Register<IHandleRequests<TestCommand>, TestAbortingHandler<TestCommand>>();
            container.Register(logger);

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);
        }

        [Test]
        [ExpectedException]
        public void it_should_throw_an_exception()
        {
            commandProcessor.Send(testCommand);
        }

        [Test]
        public void it_should_fail_the_pipeline_not_execute_it()
        {
            TestUnusedCommandHandler.ShouldRecieve(testCommand).Should().BeFalse();
        }
    }
}
