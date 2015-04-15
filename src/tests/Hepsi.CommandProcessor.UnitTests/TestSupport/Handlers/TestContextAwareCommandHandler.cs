using Common.Logging;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestContextAwareCommandHandler : RequestHandler<TestCommand>
    {
        public TestContextAwareCommandHandler(ILog logger)
            : base(logger)
        { }

        public static string TestString { get; set; }

        public override TestCommand Handle(TestCommand command)
        {
            LogContext();
            return base.Handle(command);
        }

        private void LogContext()
        {
            TestString = (string)Context.Bag["TestString"];
            Context.Bag["TestContextAwareCommandHandler"] = "I was called and set the context";
        }
    }
}
