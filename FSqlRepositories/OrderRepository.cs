using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class OrderRepository : BaseRepository<Order, long>, IOrderRepository
    {
        public OrderRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
