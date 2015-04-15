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
    public class with_a_retry_policy_check
    {
        static CommandProcessor commandProcessor;
        static readonly TestCommand testCommand = new TestCommand();
        static int retryCount;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestFailingDivideByZeroHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestFailingDivideByZeroHandler>().AsSingleton();
            container.Register<IHandleRequests<TestCommand>, ExceptionPolicyHandler<TestCommand>>().AsSingleton();
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

            TestFailingDivideByZeroHandler.ReceivedCommand = false;

            commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), policyRegistry, logger);

            try
            {
                commandProcessor.Send(testCommand);
            }
            catch (DivideByZeroException)
            {
                
            }
        }

        [Test]
        public void it_should_send_the_command_to_the_command_handler()
        {
            TestFailingDivideByZeroHandler.ShouldRecieve(testCommand).Should().BeTrue();
        }

        [Test]
        public void it_should_retry_three_times()
        {
            retryCount.Should().Be(3);
        }
    }
}
