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
    public class When_there_are_multiple_subscribers
    {
        static CommandProcessor commandProcessor;
        static readonly TestEvent myEvent = new TestEvent();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestEvent, TestEventHandler>();
            registry.Register<TestEvent, TestOtherEventHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestEvent>, TestEventHandler>("TestEventHandler");
            container.Register<IHandleRequests<TestEvent>, TestOtherEventHandler>("TestOtherHandler");
            container.Register(logger);

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);
        }

        [Test]
        public void it_should_not_throw_an_exception()
        {
            commandProcessor.Publish(myEvent);
        }

        [Test]
        public void it_should_publish_the_command_to_the_first_event_handler()
        {
            TestEventHandler.ShouldRecieve(myEvent).Should().BeTrue();
        }

        [Test]
        public void it_should_publish_the_command_to_the_second_event_handler()
        {
            TestOtherEventHandler.ShouldRecieve(myEvent).Should().BeTrue();
        }
    }
}
