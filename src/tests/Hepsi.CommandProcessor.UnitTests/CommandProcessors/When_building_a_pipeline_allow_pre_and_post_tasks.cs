using System.Linq;
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
    public class When_building_a_pipeline_preserve_the_order
    {
        static PipelineBuilder<TestCommand> pipelineBuilder;
        static IHandleRequests<TestCommand> pipeline;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestDoubleDecoratedHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestDoubleDecoratedHandler>();
            container.Register<IHandleRequests<TestCommand>, TestValidationHandler<TestCommand>>();
            container.Register<IHandleRequests<TestCommand>, TestLoggingHandler<TestCommand>>();
            container.Register(logger);

            pipelineBuilder = new PipelineBuilder<TestCommand>(registry, handlerFactory, logger);

            pipeline = pipelineBuilder.Build(new RequestContext()).First();
        }

        [Test]
        public void it_should_add_handlers_in_the_correct_sequence_into_the_chain()
        {
            TracePipeline().ToString().Should().Be("TestLoggingHandler`1|TestValidationHandler`1|TestDoubleDecoratedHandler|");
        }

        private static PipelineTracer TracePipeline()
        {
            var pipelineTracer = new PipelineTracer();
            pipeline.DescribePath(pipelineTracer);
            return pipelineTracer;
        }
    }
}
