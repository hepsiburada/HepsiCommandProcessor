using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Attributes;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestUnusedCommandHandler : RequestHandler<TestCommand>
    {
        private static TestCommand command;

        public TestUnusedCommandHandler(ILog logger)
            : base(logger)
        {
            command = null;
        }

        [TestAbortingHandler(step: 1, timing: HandlerTiming.Before)]
        public override TestCommand Handle(TestCommand command)
        {
            LogCommand(command);
            return base.Handle(command);
        }

        public static bool ShouldRecieve(TestCommand expectedCommand)
        {
            return (command != null) && (expectedCommand.Id == command.Id);
        }

        private void LogCommand(TestCommand request)
        {
            command = request;
        }
    }
}
