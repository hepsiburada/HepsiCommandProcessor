using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Extensions;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor
{
    internal class PipelineBuilder<TRequest> : IAmAPipelineBuilder<TRequest> where TRequest : class, IRequest
    {
        private readonly IAmAHandlerFactory handlerFactory;
        private readonly ILog logger;
        private readonly Interpreter<TRequest> interpreter;
        private readonly IAmALifetime instanceScope;

        internal PipelineBuilder(IAmASubscriberRegistry registry, IAmAHandlerFactory handlerFactory, ILog logger)
        {
            this.handlerFactory = handlerFactory;
            this.logger = logger;
            instanceScope = new LifetimeScope(handlerFactory);
            interpreter = new Interpreter<TRequest>(registry, handlerFactory);
        }

        public Pipelines<TRequest> Build(IRequestContext requestContext)
        {
            var handlers = interpreter.GetHandlers(typeof(TRequest));

            var pipelines = new Pipelines<TRequest>();
            handlers.Each((handler) => pipelines.Add(BuildPipeline(handler, requestContext)));

            pipelines.Each((handler) => handler.AddToLifetime(instanceScope));

            return pipelines;
        }

        public void Dispose()
        {
            instanceScope.Dispose();
        }

        IHandleRequests<TRequest> BuildPipeline(RequestHandler<TRequest> implicitHandler, IRequestContext requestContext)
        {
            implicitHandler.Context = requestContext;

            var preAttributes =
                implicitHandler.FindHandlerMethod()
                .GetOtherHandlersInPipeline()
                .Where(attribute => attribute.Timing == HandlerTiming.Before)
                .OrderByDescending(attribute => attribute.Step);

            var firstInPipeline = PushOntoPipeline(preAttributes, implicitHandler, requestContext);

            var postAttributes =
                implicitHandler.FindHandlerMethod()
                .GetOtherHandlersInPipeline()
                .Where(attribute => attribute.Timing == HandlerTiming.After)
                .OrderByDescending(attribute => attribute.Step);

            AppendToPipeline(postAttributes, implicitHandler, requestContext);

            logger.Debug(m => m("New handler pipeline created: {0}", TracePipeline(firstInPipeline)));

            return firstInPipeline;
        }

        void AppendToPipeline(IEnumerable<RequestHandlerAttribute> attributes, IHandleRequests<TRequest> implicitHandler, IRequestContext requestContext)
        {
            var lastInPipeline = implicitHandler;
            attributes.Each((attribute) =>
            {
                var decorator = new HandlerFactory<TRequest>(attribute, handlerFactory, requestContext).CreateRequestHandler();
                lastInPipeline.Successor = decorator;
                lastInPipeline = decorator;
            });
        }

        IHandleRequests<TRequest> PushOntoPipeline(IEnumerable<RequestHandlerAttribute> attributes, IHandleRequests<TRequest> lastInPipeline, IRequestContext requestContext)
        {
            attributes.Each((attribute) =>
            {
                var decorator = new HandlerFactory<TRequest>(attribute, handlerFactory, requestContext).CreateRequestHandler();
                decorator.Successor = lastInPipeline;
                lastInPipeline = decorator;
            });

            return lastInPipeline;
        }

        static PipelineTracer TracePipeline(IHandleRequests<TRequest> firstInPipeline)
        {
            var pipelineTracer = new PipelineTracer();
            firstInPipeline.DescribePath(pipelineTracer);
            return pipelineTracer;
        }
    }
}
