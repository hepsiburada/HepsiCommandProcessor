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
    public class When_a_pipeline_cleanups_its_handlers
    {
        static PipelineBuilder<TestCommand> pipelineBuilder;

        [TestFixtureSetUp]
        public void Setup()
        {
            var logger = new Mock<ILog>().Object;

            var registry = new SubscriberRegistry();
            registry.Register<TestCommand, TestPreAndPostDecoratedHandler>();
            registry.Register<TestCommand, TestLoggingHandler<TestCommand>>();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIoCHandlerFactory(container);
            container.Register<IHandleRequests<TestCommand>, TestPreAndPostDecoratedHandler>();
            container.Register<IHandleRequests<TestCommand>, TestValidationHandler<TestCommand>>();
            container.Register<IHandleRequests<TestCommand>, TestLoggingHandler<TestCommand>>();
            container.Register(logger);

            pipelineBuilder = new PipelineBuilder<TestCommand>(registry, handlerFactory, logger);

            pipelineBuilder.Build(new RequestContext());

            pipelineBuilder.Dispose();
        }

        [Test]
        public void it_should_have_called_dispose_on_instances_from_ioc()
        {
            TestPreAndPostDecoratedHandler.DisposeWasCalled.Should().BeTrue();
        }

        [Test]
        public void it_should_have_called_dispose_on_instances_from_pipeline_builder()
        {
            TestLoggingHandler<TestCommand>.DisposeWasCalled.Should().BeTrue();
        }
    }
}
