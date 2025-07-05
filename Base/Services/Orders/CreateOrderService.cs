using Base.ChannelIntergrations;
using Base.Models;
using Base.Models.Enums;
using Base.Repositories;
using Base.Utils;
using System.Data;

namespace Base.Services.Orders
{
    public class CreateOrderService : IScoped
    {
        private readonly ChannelIntergrationFactory _intergrationFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRuleRepository _paymentRuleRepository;
        private readonly IChannelMerchantRepository _channelMerchantRepository;
        private readonly IChannelMerchantOrderSummaryRepository _summaryRepository;
        private readonly IChannelIntergration _channelIntergration;

        public CreateOrderService(ChannelIntergrationFactory intergrationFactory, IOrderRepository orderRepository, IPaymentRuleRepository paymentRuleRepository, IChannelMerchantRepository channelMerchantRepository, IChannelMerchantOrderSummaryRepository summaryRepository)
        {
            _intergrationFactory = intergrationFactory;
            _orderRepository = orderRepository;
            _paymentRuleRepository = paymentRuleRepository;
            _channelMerchantRepository = channelMerchantRepository;
            _summaryRepository = summaryRepository;
        }

        public async Task<Order> CreateOrderAsync(long customerId, PaymentPlatform platform, long channelId, string orderCurrency, decimal amount, string outOrderId, string narrative, string notifyUrl, string? outNotifyUrl, string? redirectUrl = null)
        {
            // 1. 分配下单商户
            var merchant = await JudgeAccourdingRuleAsync(customerId, platform, channelId, orderCurrency, amount, notifyUrl);

            // 2. 创建订单
            var order = new Order(channelId, merchant.ChannelName, outOrderId, orderCurrency, amount, narrative, redirectUrl, merchant.Id, customerId, outNotifyUrl);
            await order.InsertAsync();

            // 2. 向商户下单
            var intergration = _intergrationFactory.CreateInstance(merchant, notifyUrl);
            var oChannelResult = await intergration.CreateOrderAsync(platform, orderCurrency, amount, order.Id.ToString(), narrative, redirectUrl);

            // 3. 更改订单状态
            order.SubmittedToChannel(oChannelResult.channelOrderId, oChannelResult.payUrl);

            return order;
        }

        private async Task<ChannelMerchant> JudgeAccourdingRuleAsync(long customerId, PaymentPlatform platform, long channelId, string orderCurrency, decimal amount, string notifyUrl)
        {
            var rule = await _paymentRuleRepository.Where(p => p.ChannelId == channelId).FirstAsync();

            var amountForRule = amount;

            if (orderCurrency.ToUpper() != rule.Currency.ToUpper())
            {
                var merchantForRate = await _channelMerchantRepository.Where(p => !p.IsDeleted).FirstAsync();
                var channelIntergrationForRate = _intergrationFactory.CreateInstance(merchantForRate, notifyUrl);
                var rate = await channelIntergrationForRate.GetExchangeRateAsync(platform, rule.Currency, orderCurrency);

                amountForRule = amount * rate;
            }
            // 判断单笔限额
            if (amountForRule >= rule.OrderAmountUpperLimit)
                throw new Exception("单笔金额超限");

            var todayInChannel = DateTime.Now.ConvertToSpecificTimezone(rule.TimezoneId);
            var records = await _summaryRepository.Where(p => p.CustomerId == customerId && p.SummaryDate >= todayInChannel.AddDays(-1)).ToListAsync();

            // 判断当日限额（按客户）
            var recordEnumerableForCustomer = records.Where(p => p.SummaryDate == todayInChannel);

            var sumAmountForCustomer = recordEnumerableForCustomer.Sum(p => p.DailyAmountSummary);
            if (sumAmountForCustomer + amountForRule >= rule.DailyAmountUpperLimit)
                throw new Exception("当日金额超限");

            // 判断交易笔数限额（按客户）
            var countForCustomer = recordEnumerableForCustomer.Count();
            if (countForCustomer >= rule.DailyCountUpperLimitForCustomer - 1)
                throw new Exception("客户当日付款总笔数超限");

            // 根据交易频率规则选出可下单的商户列表
            var lastOrderTime = DateTime.UtcNow.AddMinutes(-rule.MerchantPaymentFrequencyLowerLimit);
            var unavailableMerchantsForFrequencyRule = records.Where(p => p.LastOrderDateTime >= lastOrderTime).Select(p => p.ChannelMerchantId).ToList();
            var availableMerchants = await _channelMerchantRepository.Where(p => !p.IsDeleted && p.CustomerId == customerId && !unavailableMerchantsForFrequencyRule.Contains(p.Id)).ToListAsync();
            if (!availableMerchants.Any())
                throw new Exception("交易频率过高");

            // 根据当日限额及交易笔数限额规则选择一个商户下单
            foreach (var merchant in availableMerchants)
            {
                var recordForMerchant = recordEnumerableForCustomer.Where(p => p.ChannelMerchantId == merchant.Id).FirstOrDefault();
                if (recordForMerchant == null)
                    return merchant;
                else
                {
                    if (recordForMerchant.DailyAmountSummary + amount <= rule.DailyAmountUpperLimit || recordForMerchant.DailyOrderCount <= rule.DailyCountUpperLimitForMerchant + 1)
                        return merchant;
                }
            }

            throw new Exception("今日商户交易金额/笔数均超限额");
        }


    }
}
