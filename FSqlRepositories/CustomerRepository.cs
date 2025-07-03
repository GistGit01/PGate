using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class CustomerRepository : BaseRepository<Customer, long>, ICustomerRepository
    {
        public CustomerRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
