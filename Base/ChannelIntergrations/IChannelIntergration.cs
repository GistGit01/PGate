using Base.Models;
using Base.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Base.ChannelIntergrations
{
    public interface IChannelIntergration
    {
        Task<decimal> GetExchangeRateAsync(PaymentPlatform platform, string originCurrency, string targetCurrency);

        Task<(string channelOrderId, string payUrl)> CreateOrderAsync(PaymentPlatform platform, string currency, decimal amount, string orderId, string narrative, string? redirectUrl);

        Task<(OrderStatus status, DateTime? payTime)> QueryOrderAsync(string channelOrderId);

        Task<(string channelRefundId, DateTime refundTime)> RefundOrderAsync(string channelOrderId, decimal amount, string refundId);

        Task<RefundStatus> QueryRefundAsync(string channelRefundId);
    }
}
