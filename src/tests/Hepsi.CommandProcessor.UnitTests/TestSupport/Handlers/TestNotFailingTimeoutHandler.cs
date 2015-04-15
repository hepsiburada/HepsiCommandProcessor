using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestNotFailingTimeoutHandler : RequestHandler<TestCommand>
    {
        public TestNotFailingTimeoutHandler(ILog logger)
            : base(logger)
        { }

        public static bool CommandRecieved { get; set; }

        [TimeoutPolicy(milliseconds: 10000, step: 0, timing: HandlerTiming.Before)]
        public override TestCommand Handle(TestCommand command)
        {
            var cancellationToken = (CancellationToken)Context.Bag[TimeoutPolicyHandler<TestCommand>.CONTEXT_BAG_TIMEOUT_CANCELLATION_TOKEN];
            Task.Delay(100, cancellationToken).ContinueWith(x =>
            {
                CommandRecieved = true;
            }).Wait();
            return base.Handle(command);

        }

        public static bool ShouldRecieve(TestCommand command)
        {
            return CommandRecieved;
        }
    }
}
