using Core.Interfaces;
using Data.Models;
using Infra.Repository;

namespace Infra.UnitOfWork
{
    public class UnitOfWork //:IDisposable
    {
        private IRepository<tbTransaction> _transactionRepo;
      

        private readonly TransactionsDBContext _ctx;
        public UnitOfWork(TransactionsDBContext ctx)
        {
            _ctx = ctx;
        }
  
        public IRepository<tbTransaction> transactionRepo
        {
            get
            {
                if (_transactionRepo == null)
                {
                    _transactionRepo = new Repository<tbTransaction>(_ctx);
                }
                return _transactionRepo;
            }
        }
      
    }
}
