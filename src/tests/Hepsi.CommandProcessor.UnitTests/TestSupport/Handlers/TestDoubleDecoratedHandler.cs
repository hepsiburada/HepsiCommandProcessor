using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Attributes;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestDoubleDecoratedHandler : RequestHandler<TestCommand>
    {
        public TestDoubleDecoratedHandler(ILog logger)
            : base(logger)
        { }

        [TestLoggingHandler(step: 1)]
        [TestValidationHandler(step: 2)]
        public override TestCommand Handle(TestCommand command)
        {
            return base.Handle(command);
        }
    }
}
