using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class ChannelRepository : BaseRepository<Channel, long>, IChannelRepository
    {
        public ChannelRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
