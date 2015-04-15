using Common.Logging;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestCommandHandler : RequestHandler<TestCommand>
    {
        private static TestCommand command;

        public TestCommandHandler(ILog logger)
            : base(logger)
        {
            command = null;
        }

        public override TestCommand Handle(TestCommand command)
        {
            LogCommand(command);
            return base.Handle(command);
        }

        public static bool ShouldRecieve(TestCommand expectedCommand)
        {
            return (command != null) && (expectedCommand.Id == command.Id);
        }

        private static void LogCommand(TestCommand request)
        {
            command = request;
        }
    }
}
