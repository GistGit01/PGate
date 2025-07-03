using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class RefundRepository : BaseRepository<Refund, long>, IRefundRepository
    {
        public RefundRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
