using System;
using Common.Logging;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestLoggingHandler<TRequest> : RequestHandler<TRequest>, IDisposable where TRequest : class, IRequest
    {
        private TRequest command;
        public static bool DisposeWasCalled { get; set; }

        public TestLoggingHandler(ILog logger)
            : base(logger)
        {
            command = null;
            DisposeWasCalled = false;
        }

        public override TRequest Handle(TRequest command)
        {
            LogCommand(command);
            return base.Handle(command);
        }

        public static bool ShouldRecieve(TRequest expectedCommand)
        {
            return (expectedCommand != null);
        }

        private void LogCommand(TRequest request)
        {
            command = request;
        }

        public void Dispose()
        {
            DisposeWasCalled = true;
        }
    }
}
