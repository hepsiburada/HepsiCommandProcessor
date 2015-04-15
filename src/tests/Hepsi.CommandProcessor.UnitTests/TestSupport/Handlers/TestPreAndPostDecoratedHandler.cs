using System;
using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Attributes;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestPreAndPostDecoratedHandler : RequestHandler<TestCommand>, IDisposable
    {
        private static TestCommand command;
        public static bool DisposeWasCalled { get; set; }

        public TestPreAndPostDecoratedHandler(ILog logger)
            : base(logger)
        {
            command = null;
            DisposeWasCalled = false;
        }

        [TestPreValidationHandler(step: 2, timing: HandlerTiming.Before)]
        [TestPostLoggingHandler(step: 1, timing: HandlerTiming.After)]
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

        public void Dispose()
        {
            DisposeWasCalled = true;
        }
    }
}
