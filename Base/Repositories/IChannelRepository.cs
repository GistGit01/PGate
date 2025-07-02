using Base.Models;
using Base.Utils;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repositories
{
    public interface IChannelRepository:IBaseRepository<Channel, long>, IScoped
    {
    }
}
