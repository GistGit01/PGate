using Base.ChannelIntergrations;
using Base.Models;
using Base.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Base.Services.Orders
{
    public class CreateOrderService : IScoped
    {
        private readonly ChannelIntergrationFactory _intergrationFactory;

        public CreateOrderService(ChannelIntergrationFactory intergrationFactory)
        {
            _intergrationFactory = intergrationFactory;
        }

        public async Task<Order> CreateOrderAsync()
        {
            //var merchant = new Merchant("", 1, "", "", "");
            //var intergration = _intergrationFactory.CreateInstance(merchant);
            
        }
    }
}
