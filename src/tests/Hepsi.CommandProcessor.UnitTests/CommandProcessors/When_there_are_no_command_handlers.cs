using System;
using Common.Logging;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Moq;
using NUnit.Framework;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_there_are_no_command_handlers
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            commandProcessor = new CommandProcessor(new SubscriberRegistry(), new TinyIoCHandlerFactory(new TinyIoCContainer()), new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "No command handler was found for the typeof command Hepsi.CommandProcessor.UnitTests.TestSupport.TestCommand - a command should have only one handler.")]
        public void it_should_fail_because_multiple_recievers_found()
        {
            commandProcessor.Send(testCommand);
        }
    }
}
