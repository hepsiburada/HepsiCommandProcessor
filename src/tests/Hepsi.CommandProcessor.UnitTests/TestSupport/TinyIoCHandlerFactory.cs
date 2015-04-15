using System;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport
{
    internal class TinyIoCHandlerFactory : IAmAHandlerFactory
    {
        private readonly TinyIoCContainer container;

        public TinyIoCHandlerFactory(TinyIoCContainer container)
        {
            this.container = container;
        }

        public IHandleRequests Create(Type handlerType)
        {
            return (IHandleRequests)container.Resolve(handlerType);
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
