using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Handlers;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestFailingTimeoutHandler : RequestHandler<TestCommand>
    {
        public static bool WasCancelled { get; set; }
        public static bool TaskCompleted { get; set; }

        public TestFailingTimeoutHandler(ILog logger)
            : base(logger)
        {
        }

        [TimeoutPolicy(milliseconds: 300, step: 1, timing: HandlerTiming.Before)]
        public override TestCommand Handle(TestCommand command)
        {
            var cancellationToken = (CancellationToken)Context.Bag[TimeoutPolicyHandler<TestCommand>.CONTEXT_BAG_TIMEOUT_CANCELLATION_TOKEN];
            if (cancellationToken.IsCancellationRequested)
            {
                WasCancelled = true;
                return base.Handle(command);
            }
            try
            {
                var delay = Task.Delay(1000, cancellationToken).ContinueWith(
                    x =>
                    {
                        WasCancelled = false;
                    },
                    cancellationToken);

                Task.WaitAll(delay);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.OfType<TaskCanceledException>().Any())
                {
                    WasCancelled = true;
                    TaskCompleted = false;
                    return base.Handle(command);
                }
            }

            TaskCompleted = true;
            return base.Handle(command);
        }
    }
}
