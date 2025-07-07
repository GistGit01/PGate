using Base.ChannelIntergrations;
using Base.Models;
using Base.Repositories;
using Base.Utils;

namespace Base.Services.Orders
{
    public class RefundService : IScoped
    {
        private readonly ChannelIntergrationFactory _intergrationFactory;
        private readonly IChannelMerchantRepository _merchantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRefundRepository _refundRepository;

        public RefundService(ChannelIntergrationFactory intergrationFactory, IChannelMerchantRepository merchantRepository, IOrderRepository orderRepository, IRefundRepository refundRepository)
        {
            _intergrationFactory = intergrationFactory;
            _merchantRepository = merchantRepository;
            _orderRepository = orderRepository;
            _refundRepository = refundRepository;
        }

        public async Task<Refund> RefundAsync(long customerId, long orderId, decimal amount)
        {
            var order = _orderRepository.Find(orderId);

            if (order == null || order?.CustomerId != customerId)
                throw new Exception("订单不存在");

            order.Refund(amount);
            var merchant = _merchantRepository.Find(order.ChannelMerchantId);

            var refund = new Refund(order.ChannelId, order.ChannelName, order.Id, order.ChannelOrderId, order.Platform.Value, amount, order.Currency);

            var intergration = _intergrationFactory.CreateInstance(merchant, "");
            var channelRefundResult = await intergration.RefundOrderAsync(order.ChannelOrderId, amount, refund.Id.ToString());

            refund.Refunded(channelRefundResult.channelRefundId);

            await order.UpdateAsync();
            await refund.InsertAsync();

            return refund;

        }
    }
}
