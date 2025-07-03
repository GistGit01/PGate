using Base.Models;
using Base.Utils;
using FreeSql;

namespace Base.Repositories
{
    public interface ITransactionRepository :IBaseRepository<Transaction, long>, IScoped
    {
    }
}
