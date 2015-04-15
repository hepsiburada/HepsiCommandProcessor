using System.Linq;
using Common.Logging;
using FluentAssertions;
using Hepsi.CommandProcessor.UnitTests.TestSupport;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Persistence;
using Moq;
using NUnit.Framework;

namespace Hepsi.CommandProcessor.UnitTests.CommandProcessors
{
    [TestFixture]
    public class When_finding_an_handler_that_has_dependencies
    {
        static PipelineBuilder<TestCommand> pipelineBuilder;
        static IHandleRequests<TestCommand> pipeline;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestCommandHandler>();
            var handlerFactory = new TestHandlerFactory<TestCommand, TestDependentCommandHandler>(() => 
                new TestDependentCommandHandler(new FakeRepository<TestAggregate>(new FakeSession()), logger));

            pipelineBuilder = new PipelineBuilder<TestCommand>(registry, handlerFactory, logger);

            pipeline = pipelineBuilder.Build(new RequestContext()).First();
        }

        [Test]
        public void it_should_return_the_command_handler_as_the_implicit_handler()
        {
            pipeline.Should().BeAssignableTo<TestDependentCommandHandler>();
        }

        [Test]
        public void it_should_be_the_only_element_in_the_chain()
        {
            TracePipeline().ToString().Should().Be("TestDependentCommandHandler|");
        }

        private static PipelineTracer TracePipeline()
        {
            var pipelineTracer = new PipelineTracer();
            pipeline.DescribePath(pipelineTracer);
            return pipelineTracer;
        }
    }
}
