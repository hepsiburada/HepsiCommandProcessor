using System.Linq;
using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Moq;
using NUnit.Framework;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_building_an_handler_for_a_command
    {
        static PipelineBuilder<TestCommand> pipelineBuilder;
        static IHandleRequests<TestCommand> pipeline;
        static RequestContext requestContext;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestCommandHandler>();
            var handlerFactory = new TestHandlerFactory<TestCommand, TestCommandHandler>(() => new TestCommandHandler(logger));
            requestContext = new RequestContext();

            pipelineBuilder = new PipelineBuilder<TestCommand>(registry, handlerFactory, logger);

            pipeline = pipelineBuilder.Build(requestContext).First();
        }

        [Test]
        public void it_should_have_set_the_context_on_the_handler()
        {
            pipeline.Context.Should().NotBeNull();
        }

        [Test]
        public void it_should_use_the_context_that_is_passed_in()
        {
            pipeline.Context.Should().Be(requestContext);
        }
    }
}
