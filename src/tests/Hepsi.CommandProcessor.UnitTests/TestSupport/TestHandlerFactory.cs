using System;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport
{
    internal class TestHandlerFactory<TRequest, TRequestHandler> : IAmAHandlerFactory
        where TRequest : class, IRequest
        where TRequestHandler : class, IHandleRequests<TRequest>
    {
        private readonly Func<TRequestHandler> factoryMethod;

        public TestHandlerFactory(Func<TRequestHandler> factoryMethod)
        {
            this.factoryMethod = factoryMethod;
        }

        public IHandleRequests Create(Type handlerType)
        {
            return factoryMethod();
        }

        public void Release(IHandleRequests handler)
        {
            var disposable = handler as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }

            handler = null;
        }
    }
}
