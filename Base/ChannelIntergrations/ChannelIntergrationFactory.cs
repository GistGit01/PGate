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
        private readonly string _notifyUrl;

        public ChannelIntergrationFactory(string notifyUrl)
        {
            _notifyUrl = notifyUrl;
        }

        public IChannelIntergration CreateInstance(ChannelMerchant merchant)
        {
            switch (merchant.ChannelName.ToUpper())
            {
                case "OMIPAY":
                    return new OmipayIntergration(merchant, _notifyUrl);
                default:
                    throw new Exception("Channel not support");
            }
        }
    }
}
