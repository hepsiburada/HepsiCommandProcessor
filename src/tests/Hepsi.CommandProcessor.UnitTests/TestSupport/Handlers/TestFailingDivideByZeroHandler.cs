using System;
using Common.Logging;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestFailingDivideByZeroHandler : RequestHandler<TestCommand>
    {
        public TestFailingDivideByZeroHandler(ILog logger)
            : base(logger)
        { }

        public static bool ReceivedCommand { get; set; }

        static TestFailingDivideByZeroHandler()
        {
            ReceivedCommand = false;
        }

        [UsePolicy(policy: "TestDivideByZeroPolicy", step: 1)]
        public override TestCommand Handle(TestCommand command)
        {
            ReceivedCommand = true;
            throw new DivideByZeroException();
        }

        public static bool ShouldRecieve(TestCommand testCommand)
        {
            return ReceivedCommand;
        }
    }
}
