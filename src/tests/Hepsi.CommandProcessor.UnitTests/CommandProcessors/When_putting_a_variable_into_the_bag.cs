using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_putting_a_variable_into_the_bag
    {
        const string BagMessage = "I am a test of the context bag";
        static RequestContext requestContext;
        static CommandProcessor commandProcessor;
        static TestCommand testCommand;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestContextAwareCommandHandler>();
            var handlerFactory = new TestHandlerFactory<TestCommand, TestContextAwareCommandHandler>(() => new TestContextAwareCommandHandler(logger));
            requestContext = new RequestContext();
            testCommand = new TestCommand();
            TestContextAwareCommandHandler.TestString = null;

            var requestContextFactory = new Mock<IAmARequestContextFactory>();
            requestContextFactory.Setup(r => r.Create()).Returns(requestContext);

            commandProcessor = new CommandProcessor(registry, handlerFactory, requestContextFactory.Object, new PolicyRegistry(), logger);

            requestContext.Bag["TestString"] = BagMessage;

            commandProcessor.Send(testCommand);
        }

        [Test]
        public void it_should_have_correct_data()
        {
            TestContextAwareCommandHandler.TestString.Should().Be(BagMessage);
        }

        [Test]
        public void it_should_have_data_is_populated_by_handler()
        {
            ((string)requestContext.Bag["TestContextAwareCommandHandler"]).Should().Be("I was called and set the context");
        }
    }
}
