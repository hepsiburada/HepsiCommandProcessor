using System.Collections.Generic;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Persistence
{
    internal interface IUnitOfWork
    {
        void Add<T>(T aggregate);
        void Commit();
        T Load<T>(int id) where T : IAmAnAggregate;
        IEnumerable<T> Query<T>() where T : IAmAnAggregate;
    }
}
