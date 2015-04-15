using System;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Persistence
{
    internal interface IRepository<T> where T : IAmAnAggregate
    {
        void Add(T aggregate);
        T this[Guid id] { get; }
        IUnitOfWork UnitOfWork { set; }
    }
}
