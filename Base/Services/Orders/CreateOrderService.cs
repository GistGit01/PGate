using Base.ChannelIntergrations;
using Base.Models;
using Base.Models.Enums;
using Base.Repositories;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRuleRepository _paymentRuleRepository;
        private readonly IChannelMerchantRepository _channelMerchantRepository;
        private readonly IChannelIntergration _channelIntergration;

        public CreateOrderService(ChannelIntergrationFactory intergrationFactory, IOrderRepository orderRepository, IPaymentRuleRepository paymentRuleRepository, IChannelMerchantRepository channelMerchantRepository)
        {
            _intergrationFactory = intergrationFactory;
            _orderRepository = orderRepository;
            _paymentRuleRepository = paymentRuleRepository;
            _channelMerchantRepository = channelMerchantRepository;
        }

        public async Task<Order> CreateOrderAsync(long customerId, PaymentPlatform platform, long channelId, string orderCurrency, decimal amount)
        {
            // 1. 分配下单商户
            /*
             * 分配策略：根据下单记录，
             */

            // 2. 向商户下单            
        }

        private async Task<ChannelMerchant?> JudgeAccourdingRuleAsync(long customerId, PaymentPlatform platform, long channelId, string orderCurrency, decimal amount)
        {
            var rule = await _paymentRuleRepository.Where(p => p.ChannelId == channelId).FirstAsync();

            var amountForRule = amount;

            if(orderCurrency.ToUpper() != rule.Currency.ToUpper())
            {
                var merchantForRate = await _channelMerchantRepository.Where(p => !p.IsDeleted).FirstAsync();
                var channelIntergrationForRate = _intergrationFactory.CreateInstance(merchantForRate);
                var rate = await channelIntergrationForRate.GetExchangeRateAsync(platform, rule.Currency, orderCurrency);

                amountForRule = amount * rate;
            }
            // 判断单笔限额
            if (amountForRule >= rule.OrderAmountUpperLimit)
                throw new Exception("单笔金额超限");

            // 判断当日限额（按客户）
            var nowInChannelTimezone = DateTime.UtcNow.ConvertToSpecificTimezone(rule.TimezoneName);
            var totalMinutes = nowInChannelTimezone.TimeOfDay.TotalMinutes;
            var beginDateTimeForSum = DateTime.UtcNow.AddMinutes(-totalMinutes);
                        
            var channelMerchants = await _channelMerchantRepository.Where(p => !p.IsDeleted && p.CustomerId == customerId).ToListAsync();
            var sumAmount = await _orderRepository.Where(p => p.Status != OrderStatus.Created
                                                            && p.ChannelId == channelId 
                                                            && p.PayTime >= beginDateTimeForSum 
                                                            && channelMerchants.Select(q => q.Id).Contains(p.ChannelMerchantId))
                                                  .SumAsync(p => p.Amount);
            if (sumAmount + amountForRule >= rule.DailyAmountUpperLimit)
                throw new Exception("当日金额超限");

            // 判断交易频率
            // TODO:
        }
    }
}
