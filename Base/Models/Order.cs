using Base.Models.Enums;
using FreeSql.DataAnnotations;

namespace Base.Models
{
    [Table(Name ="order")]
    public class Order:Entity<Order>
    {
        private Order() {}
        public Order(long channelId, string channelName, string channelOrderId, string outOrderId, string currency, decimal amount,
            string narrative, string? payUrl, string? redirectUrl, long channelMerchantId, long customerId)
        {
            ChannelId = channelId;
            ChannelName = channelName;
            ChannelOrderId = channelOrderId;
            OutOrderId = outOrderId;
            Status = OrderStatus.Created;
            Currency = currency;
            Amount = amount;
            Narrative = narrative;
            PayUrl = payUrl;
            RedirectUrl = redirectUrl;
            ChannelMerchantId = channelMerchantId;
            CustomerId = customerId;
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

        public DateTime? PayTime { get; private set; }

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
