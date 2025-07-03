using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class AccountRepository : BaseRepository<Account, long>, IAccountRepository
    {
        public AccountRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
