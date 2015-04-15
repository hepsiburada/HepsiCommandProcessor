using System;
using Hepsi.CommandProcessor.Messaging;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport
{
    internal class TestMessageMapperFactory : IAmAMessageMapperFactory
    {
        private readonly Func<IAmAMessageMapper> factoryMethod;

        public TestMessageMapperFactory(Func<IAmAMessageMapper> factoryMethod)
        {
            this.factoryMethod = factoryMethod;
        }

        public IAmAMessageMapper Create(Type messageMapperType)
        {
            return factoryMethod();
        }
    }
}
