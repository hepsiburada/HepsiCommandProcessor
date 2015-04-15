using System;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport.Persistence
{
    internal class FakeRepository<T> : IRepository<T> where T : IAmAnAggregate
    {
        private readonly IUnitOfWork _unitOfWork;

        public FakeRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(T aggregate)
        {
            _unitOfWork.Add(aggregate);
        }

        public T this[Guid id]
        {
            get { throw new NotImplementedException(); }
        }

        public IUnitOfWork UnitOfWork
        {
            set { throw new NotImplementedException(); }
        }
    }
}
