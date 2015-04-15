using Common.Logging;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestEventHandler : RequestHandler<TestEvent>
    {
        private static TestEvent receivedEvent;


        public TestEventHandler(ILog logger)
            : base(logger)
        {
            receivedEvent = null;
        }

        public override TestEvent Handle(TestEvent command)
        {
            LogEvent(command);
            return command;
        }

        private static void LogEvent(TestEvent @event)
        {
            receivedEvent = @event;
        }

        public static bool ShouldRecieve(TestEvent myEvent)
        {
            return receivedEvent.Id == myEvent.Id;
        }
    }
}
