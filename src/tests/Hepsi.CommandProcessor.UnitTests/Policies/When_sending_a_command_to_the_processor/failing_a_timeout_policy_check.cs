using System;
using System.Linq;
using System.Threading.Tasks;
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
        static AggregateException aggregateException;

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

            try
            {
                commandProcessor.Send(testCommand);
            }
            catch (AggregateException ex)
            {
                aggregateException = ex;
            }
        }

        [Test]
        public void it_should_throw_a_timeout_exception()
        {
            aggregateException.InnerExceptions.First().Should().BeOfType<TimeoutException>();
        }

        [Test]
        public void it_should_cancel_handler_task()
        {
            Task.WaitAll(Task.Delay(100));
            TestFailingTimeoutHandler.WasCancelled.Should().BeTrue();
        }

        [Test]
        public void it_should_not_complete_the_task()
        {
            Task.WaitAll(Task.Delay(100));
            TestFailingTimeoutHandler.TaskCompleted.Should().BeFalse();
        }
    }
}
