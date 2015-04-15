using System;
using Hepsi.CommandProcessor.Attributes;
using Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Attributes
{
    internal class TestValidationHandlerAttribute : RequestHandlerAttribute
    {
        public TestValidationHandlerAttribute(int step)
            : base(step)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(TestValidationHandler<>);
        }
    }
}
