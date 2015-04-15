using Common.Logging;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestValidationHandler<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        private static TRequest command;

        public TestValidationHandler(ILog logger)
            : base(logger)
        {
            command = null;
        }

        public override TRequest Handle(TRequest command)
        {
            LogCommand(command);
            return base.Handle(command);
        }

        public static bool ShouldRecieve(TRequest expectedCommand)
        {
            return (command != null);
        }

        private void LogCommand(TRequest request)
        {
            command = request;
        }
    }
}
