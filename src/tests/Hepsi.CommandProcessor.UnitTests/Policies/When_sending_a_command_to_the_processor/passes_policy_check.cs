using System;
using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;
using Polly;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.Policies.When_sending_a_command_to_the_processor
{
    [TestFixture]
    public class passes_policy_check
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();
        static int retryCount;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestNotFailingDivideByZeroHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestNotFailingDivideByZeroHandler>("TestNotFailingDivideByZeroHandler");
            container.Register<IHandleRequests<TestCommand>, ExceptionPolicyHandler<TestCommand>>("ExceptionPolicyHandler");
            container.Register(logger);

            var policyRegistry = new PolicyRegistry();

            var policy = Policy
                .Handle<DivideByZeroException>()
                .WaitAndRetry(new[]
                {
                    100.Milliseconds(),
                    200.Milliseconds(),
                    300.Milliseconds()
                }, (exception, timeSpan) =>
                {
                    retryCount++;
                });
            policyRegistry.Add("TestDivideByZeroPolicy", policy);

            TestNotFailingDivideByZeroHandler.ReceivedCommand = false;

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), policyRegistry, logger);

            commandProcessor.Send(testCommand);
        }

        [Test]
        public void it_should_send_the_command_to_the_command_handler()
        {
            TestNotFailingDivideByZeroHandler.ShouldRecieve(testCommand).Should().BeTrue();
        }

        [Test]
        public void it_should_not_retry()
        {
            retryCount.Should().Be(0);
        }
    }
}
