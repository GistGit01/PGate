using Base.Models.Enums;
using FreeSql.DataAnnotations;

namespace Base.Models
{
    [Table(Name ="order")]
    public class Order:Entity<Order>
    {
        private Order() {}
        public Order(long channelId, string channelName, string outOrderId, string currency, decimal amount,
            string narrative, string? redirectUrl, long channelMerchantId, long customerId)
        {
            ChannelId = channelId;
            ChannelName = channelName;
            ChannelOrderId = "";
            OutOrderId = outOrderId;
            Status = OrderStatus.Created;
            Currency = currency;
            Amount = amount;
            Narrative = narrative;
            RedirectUrl = redirectUrl;
            ChannelMerchantId = channelMerchantId;
            CustomerId = customerId;
        }

        public void SubmittedToChannel(string channelOrderId, string payUrl = "")
        {
            if (this.Status != OrderStatus.Created)
                throw new Exception("订单状态错误");

            this.Status = OrderStatus.SubmittedToChannel;
            this.PayUrl = payUrl;
        }

        public void Paid(DateTime payTime)
        {
            if (this.Status != OrderStatus.SubmittedToChannel)
                throw new Exception("订单状态错误");
            this.Status = OrderStatus.Paid;
            this.PayTime = payTime;
        }

        public void Settled()
        {
            if (this.Status != OrderStatus.Paid)
                throw new Exception("订单状态错误");
            this.Status = OrderStatus.Settled;
        }

        public long ChannelId { get; private set; }

        [Column(StringLength = 20)]
        public string ChannelName { get; private set; }

        [Column(StringLength = 50)]
        public string ChannelOrderId { get; private set; }

        public long ChannelMerchantId { get; private set; }

        public long CustomerId { get; private set; }

        [Column(StringLength = 50)]
        public string OutOrderId { get; private set; }

        public DateTime PayTime { get; private set; } = DateTime.MinValue;

        public OrderStatus Status { get; private set; }

        [Column(StringLength = 10)]
        public string Currency { get; private set; } = null!;

        public decimal Amount { get; private set; }

        public PaymentPlatform? Platform { get; private set; }

        [Column(StringLength = 10)]
        public string PayCurrency { get; private set; }

        public decimal PayAmount { get; private set; }

        public decimal FeeAmount { get; private set; }

        public string Narrative { get; private set; }

        public string? PayUrl { get; private set; }

        public string? RedirectUrl { get; private set; }
    }
}
