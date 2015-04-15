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
    public class When_building_a_pipeline_allow_pre_and_post_tasks
    {
        static PipelineBuilder<TestCommand> pipelineBuilder;
        static IHandleRequests<TestCommand> pipeline;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestPreAndPostDecoratedHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestPreAndPostDecoratedHandler>();
            container.Register<IHandleRequests<TestCommand>, TestValidationHandler<TestCommand>>();
            container.Register<IHandleRequests<TestCommand>, TestLoggingHandler<TestCommand>>();
            container.Register(logger);

            pipelineBuilder = new PipelineBuilder<TestCommand>(registry, handlerFactory, logger);

            pipeline = pipelineBuilder.Build(new RequestContext()).First();
        }

        [Test]
        public void it_should_add_handlers_in_the_correct_sequence_into_the_chain()
        {
            TracePipeline().ToString().Should().Be("TestValidationHandler`1|TestPreAndPostDecoratedHandler|TestLoggingHandler`1|");
        }

        private static PipelineTracer TracePipeline()
        {
            var pipelineTracer = new PipelineTracer();
            pipeline.DescribePath(pipelineTracer);
            return pipelineTracer;
        }
    }
}
