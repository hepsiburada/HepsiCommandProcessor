using System;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Attributes
{
    internal class TestLoggingHandlerAttribute : RequestHandlerAttribute
    {
        public TestLoggingHandlerAttribute(int step)
            : base(step)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(TestLoggingHandler<>);
        }
    }
}
