using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_publishing_an_event_to_the_processor
    {
        static CommandProcessor commandProcessor;
        static readonly TestEvent myEvent = new TestEvent();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestEvent, TestEventHandler>();
            var handlerFactory = new TestHandlerFactory<TestEvent, TestEventHandler>(() => new TestEventHandler(logger));

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);

            commandProcessor.Publish(myEvent);
        }

        [Test]
        public void it_should_publish_the_command_to_the_event_handler()
        {
            TestEventHandler.ShouldRecieve(myEvent).Should().BeTrue();
        }
    }
}
