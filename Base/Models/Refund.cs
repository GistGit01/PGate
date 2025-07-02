
using Base.Models.Enums;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    [Table(Name = "refund")]
    public class Refund : Entity<Refund>
    {
        public Refund(long channelId, string channelName, long orderId, string channelOrderId, PaymentPlatform platform, decimal refundAmount, decimal feeAmount, string currency)
        {
            ChannelId = channelId;
            ChannelName = channelName;
            OrderId = orderId;
            ChannelOrderId = channelOrderId;
            Platform = platform;
            RefundAmount = refundAmount;
            FeeAmount = feeAmount;
            Currency = currency;
            Status = RefundStatus.Created;
        }

        public void Refunded(string orderRefundId)
        {
            if (Status != RefundStatus.Created)
                throw new Exception("退款单状态错误");
            Status = RefundStatus.Refunded;
        }

        public void Settled()
        {
            if (Status != RefundStatus.Refunded)
                throw new Exception("退款单状态错误");
            Status = RefundStatus.Settled;
        }

        private Refund() {}

        public long ChannelId { get; private set; }

        [Column(StringLength = 50)]
        public string ChannelName { get; private set; }

        public long OrderId { get; private set; }

        [Column(StringLength = 50)]
        public string ChannelOrderId { get; private set; }

        public PaymentPlatform Platform { get; private set; }

        public decimal RefundAmount { get; private set; }

        public decimal FeeAmount { get; private set; }

        [Column(StringLength = 10)]
        public string Currency { get; private set; }

        public RefundStatus Status { get; private set; }

        [Column(StringLength = 50)]
        public string PlatformRefundId { get; private set; } = "";
    }
}
