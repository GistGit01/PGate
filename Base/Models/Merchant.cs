using FreeSql.DataAnnotations;

namespace Base.Models
{
    [Table(Name ="merchant")]
    public class Merchant :Entity<Merchant>
    {
        private Merchant() {}

        public Merchant(string name, long channelId, string channelName,  string channelSecretKey, string channelMerchantNumber)
        {
            Name = name;
            ChannelId = channelId;
            ChannelSecretKey = channelSecretKey;
            ChannelMerchantNumber = channelMerchantNumber;
            ChannelName = channelName;
        }

        [Column(StringLength = 200)]
        public string Name { get; private set; }

        public long ChannelId { get; private set; }

        public string ChannelName { get; private set; }

        [Column(StringLength = 100)]
        public string ChannelSecretKey { get; private set; }

        [Column(StringLength = 30)]
        public string ChannelMerchantNumber { get; private set; }

        
    }
}
