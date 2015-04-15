using System;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.Handlers;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Attributes
{
    internal class TestPreValidationHandlerAttribute : RequestHandlerAttribute
    {
        public TestPreValidationHandlerAttribute(int step, HandlerTiming timing)
            : base(step, timing)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(TestValidationHandler<>);
        }
    }
}
