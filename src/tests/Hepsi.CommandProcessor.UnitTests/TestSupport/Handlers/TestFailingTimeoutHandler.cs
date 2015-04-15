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
        public TestFailingTimeoutHandler(ILog logger)
            : base(logger)
        {
        }

        public static bool WasCancelled { get; set; }
        public static bool TaskCompleted { get; set; }


        [TimeoutPolicy(milliseconds: 100, step: 1, timing: HandlerTiming.Before)]
        public override TestCommand Handle(TestCommand command)
        {
            var ct = (CancellationToken)Context.Bag[TimeoutPolicyHandler<TestCommand>.CONTEXT_BAG_TIMEOUT_CANCELLATION_TOKEN];
            if (ct.IsCancellationRequested)
            {
                //already died
                WasCancelled = true;
                return base.Handle(command);
            }
            try
            {
                var delay = Task.Delay(1000, ct).ContinueWith(
                    x =>
                    {
                        WasCancelled = false;
                    },
                    ct);

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
