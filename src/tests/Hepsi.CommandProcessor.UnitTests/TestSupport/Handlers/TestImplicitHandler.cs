using Common.Logging;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Attributes;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestImplicitHandler : RequestHandler<TestCommand>
    {
        public TestImplicitHandler(ILog logger)
            : base(logger)
        { }

        [TestLoggingHandler(step: 1)]
        public override TestCommand Handle(TestCommand command)
        {
            return base.Handle(command);
        }
    }
}
