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
    public class When_an_handler_is_part_of_a_pipeline
    {
        static PipelineBuilder<TestCommand> pipelineBuilder;
        static IHandleRequests<TestCommand> pipeline;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestImplicitHandler>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestImplicitHandler>();
            container.Register<IHandleRequests<TestCommand>, TestLoggingHandler<TestCommand>>();
            container.Register(logger);

            pipelineBuilder = new PipelineBuilder<TestCommand>(registry, handlerFactory, logger);

            pipeline = pipelineBuilder.Build(new RequestContext()).First();
        }

        [Test]
        public void it_should_include_my_command_handler_filter_in_the_chain()
        {
            TracePipeline().ToString().Should().Contain("TestImplicitHandler");
        }

        [Test]
        public void it_should_include_my_logging_handler_in_the_chain()
        {
            TracePipeline().ToString().Should().Contain("TestLoggingHandler");
        }

        private static PipelineTracer TracePipeline()
        {
            var pipelineTracer = new PipelineTracer();
            pipeline.DescribePath(pipelineTracer);
            return pipelineTracer;
        }
    }
}
