using Base.Models;
using Base.Utils;
using FreeSql;

namespace Base.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order, long>, IScoped
    {
    }
}
