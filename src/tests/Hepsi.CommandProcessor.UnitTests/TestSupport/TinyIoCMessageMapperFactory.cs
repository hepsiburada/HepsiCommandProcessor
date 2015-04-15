using System;
using Hepsi.CommandProcessor.Messaging;
using TinyIoC;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport
{
    internal class TinyIoCMessageMapperFactory : IAmAMessageMapperFactory
    {
        private readonly TinyIoCContainer container;

        public TinyIoCMessageMapperFactory(TinyIoCContainer container)
        {
            this.container = container;
        }

        public IAmAMessageMapper Create(Type messageMapperType)
        {
            return (IAmAMessageMapper)container.Resolve(messageMapperType);
        }
    }
}
