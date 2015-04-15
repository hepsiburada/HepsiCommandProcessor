using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.Policies.When_sending_a_command_to_the_processor
{
    [TestFixture]
    public class passing_a_timeout_policy_check
    {
        static readonly TestCommand testCommand = new TestCommand();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestNotFailingTimeoutHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestNotFailingTimeoutHandler>().AsSingleton();
            container.Register<IHandleRequests<TestCommand>, TimeoutPolicyHandler<TestCommand>>().AsSingleton();
            container.Register(logger);

            var commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);

            commandProcessor.Send(testCommand);
        }

        [Test]
        public void it_should_complete_the_command_before_an_exception()
        {
            TestNotFailingTimeoutHandler.ShouldRecieve(testCommand);
        }
    }
}
