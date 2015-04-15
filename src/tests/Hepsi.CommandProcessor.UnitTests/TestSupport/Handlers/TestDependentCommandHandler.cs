using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Persistence;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestDependentCommandHandler : RequestHandler<TestCommand>
    {
        private readonly IRepository<TestAggregate> repository;
        private static TestCommand command;

        public TestDependentCommandHandler(IRepository<TestAggregate> repository, ILog logger)
            : base(logger)
        {
            this.repository = repository;
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

        private void LogCommand(TestCommand request)
        {
            command = request;
        }
    }
}