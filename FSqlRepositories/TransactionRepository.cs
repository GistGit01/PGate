using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class TransactionRepository : BaseRepository<Transaction, long>,ITransactionRepository
    {
        public TransactionRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
