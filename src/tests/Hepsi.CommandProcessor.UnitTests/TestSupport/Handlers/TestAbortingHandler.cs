using System;
using Common.Logging;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Handlers
{
    internal class TestAbortingHandler<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        public TestAbortingHandler(ILog logger)
            : base(logger)
        { }

        public override TRequest Handle(TRequest command)
        {
            throw new Exception("Aborting chain");
        }
    }
}
