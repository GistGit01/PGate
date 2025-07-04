using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class ChannelMerchantOrderSummaryRepository : BaseRepository<ChannelMerchantOrderSummary, long>, IChannelMerchantOrderSummaryRepository
    {
        public ChannelMerchantOrderSummaryRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
