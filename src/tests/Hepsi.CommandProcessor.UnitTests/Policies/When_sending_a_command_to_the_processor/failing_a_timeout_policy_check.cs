using System;
using System.Linq;
using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.Policies.When_sending_a_command_to_the_processor
{
    [TestFixture]
    public class failing_a_timeout_policy_check
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestFailingTimeoutHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register(logger);
            container.Register<IHandleRequests<TestCommand>, TestFailingTimeoutHandler>().AsSingleton();
            container.Register<IHandleRequests<TestCommand>, TimeoutPolicyHandler<TestCommand>>().AsSingleton();

            TestFailingTimeoutHandler.WasCancelled = false;
            TestFailingTimeoutHandler.TaskCompleted = true;

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), logger);
        }

        [Test]
        public void it_should_throw_a_timeout_exception()
        {
            var ex = Assert.Throws<AggregateException>(() => commandProcessor.Send(testCommand));
            ex.InnerExceptions.First().Should().BeOfType<TimeoutException>();

            TestFailingTimeoutHandler.WasCancelled.Should().BeTrue();
            TestFailingTimeoutHandler.TaskCompleted.Should().BeFalse();
        }
    }
}
