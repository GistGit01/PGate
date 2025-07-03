using Base.Models;
using Base.Repositories;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSqlRepositories
{
    public class ChannelMerchantRepository : BaseRepository<ChannelMerchant, long>, IChannelMerchantRepository
    {
        public ChannelMerchantRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
