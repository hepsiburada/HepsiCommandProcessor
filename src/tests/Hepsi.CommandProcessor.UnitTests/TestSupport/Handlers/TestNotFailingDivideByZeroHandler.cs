using Common.Logging;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestNotFailingDivideByZeroHandler : RequestHandler<TestCommand>
    {
        public TestNotFailingDivideByZeroHandler(ILog logger)
            : base(logger)
        { }

        public static bool ReceivedCommand { get; set; }

        static TestNotFailingDivideByZeroHandler()
        {
            ReceivedCommand = false;
        }

        [UsePolicy(policy: "TestDivideByZeroPolicy", step: 1)]
        public override TestCommand Handle(TestCommand command)
        {
            ReceivedCommand = true;
            return base.Handle(command);
        }

        public static bool ShouldRecieve(TestCommand myCommand)
        {
            return ReceivedCommand;
        }
    }
}
