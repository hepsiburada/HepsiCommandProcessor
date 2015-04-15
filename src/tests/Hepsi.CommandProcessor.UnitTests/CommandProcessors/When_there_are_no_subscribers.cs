using Common.Logging;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_there_are_no_subscribers
    {
        static CommandProcessor commandProcessor;
        static readonly TestEvent myEvent = new TestEvent();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            var handlerFactory = new TestHandlerFactory<TestEvent, TestEventHandler>(() => new TestEventHandler(logger));

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);
        }

        [Test]
        public void it_should_not_throw_an_exception()
        {
            commandProcessor.Publish(myEvent);
        }
    }
}
