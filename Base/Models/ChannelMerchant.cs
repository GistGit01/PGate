using FreeSql.DataAnnotations;

namespace Base.Models
{
    [Table(Name ="channel_merchant")]
    public class ChannelMerchant :Entity<ChannelMerchant>
    {
        private ChannelMerchant() {}

        public ChannelMerchant(string name, long channelId, string channelName,  string channelSecretKey, string channelMerchantNumber, long customerId)
        {
            Name = name;
            ChannelId = channelId;
            ChannelSecretKey = channelSecretKey;
            ChannelMerchantNumber = channelMerchantNumber;
            ChannelName = channelName;
            CustomerId = customerId;
        }

        [Column(StringLength = 200)]
        public string Name { get; private set; }

        public long ChannelId { get; private set; }

        public string ChannelName { get; private set; }

        [Column(StringLength = 100)]
        public string ChannelSecretKey { get; private set; }

        [Column(StringLength = 30)]
        public string ChannelMerchantNumber { get; private set; }

        public long CustomerId { get; set; }
    }
}
