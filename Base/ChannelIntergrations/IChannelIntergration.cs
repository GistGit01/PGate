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
        Task<Order> CreateOrderAsync(PaymentPlatform platform, string currency, decimal amount, string orderId, string narrative, string? redirectUrl);

        OrderStatus QueryOrder(string channelOrderId);

        Refund RefundOrder(string channelOrderId, decimal amount, string refundId);

        RefundStatus QueryRefund(string channelRefundId);
    }
}
