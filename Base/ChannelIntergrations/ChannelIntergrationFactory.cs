using Base.ChannelIntergrations.Omipay;
using Base.Models;
using Base.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ChannelIntergrations
{
    public class ChannelIntergrationFactory : ISingleton
    {
        public ChannelIntergrationFactory()
        {
        }

        public IChannelIntergration CreateInstance(ChannelMerchant merchant, string notifyUrl)
        {
            switch (merchant.ChannelName.ToUpper())
            {
                case "OMIPAY":
                    return new OmipayIntergration(merchant, notifyUrl);
                default:
                    throw new Exception("Channel not support");
            }
        }
    }
}
