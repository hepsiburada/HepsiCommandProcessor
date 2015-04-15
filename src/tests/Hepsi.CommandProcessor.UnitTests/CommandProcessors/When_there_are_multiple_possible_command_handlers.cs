using System;
using Common.Logging;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_there_are_multiple_possible_command_handlers
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestCommandHandler>();
            registry.Register<TestCommand, TestImplicitHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestCommandHandler>("DefaultHandler");
            container.Register<IHandleRequests<TestCommand>, TestImplicitHandler>("ImplicitHandler");
            container.Register<IHandleRequests<TestCommand>, TestLoggingHandler<TestCommand>>();
            container.Register(logger);

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "More than one handler was found for the typeof command Hepsi.CommandProcessor.UnitTests.TestSupport.TestCommand - a command should only have one handler.")]
        public void it_should_fail_because_multiple_recievers_found()
        {
            commandProcessor.Send(testCommand);
        }
    }
}
