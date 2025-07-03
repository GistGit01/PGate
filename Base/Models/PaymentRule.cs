using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    [Table(Name = "payment_rule")]
    public class PaymentRule : Entity<PaymentRule>
    {
        public PaymentRule(long channelId, string channelName, int merchantPaymentFrequencyLowerLimit, decimal orderAmountUpperLimit, decimal dailyAmountUpperLimit, string timezoneName, int availableTimeRangeStartHour, int availableTimeRangeEndHour, string currency)
        {
            ChannelId = channelId;
            ChannelName = channelName;
            MerchantPaymentFrequencyLowerLimit = merchantPaymentFrequencyLowerLimit;
            OrderAmountUpperLimit = orderAmountUpperLimit;
            DailyAmountUpperLimit = dailyAmountUpperLimit;
            TimezoneName = timezoneName;
            AvailableTimeRangeStartHour = availableTimeRangeStartHour;
            AvailableTimeRangeEndHour = availableTimeRangeEndHour;
            Currency = currency;
        }

        private PaymentRule() { }

        public long ChannelId { get; private set; }

        [Column(StringLength = 50)]
        public string ChannelName { get; private set; }

        public int MerchantPaymentFrequencyLowerLimit { get; private set; }

        [Column(StringLength = 10)]
        public string Currency { get; private set; }

        public decimal OrderAmountUpperLimit { get; private set; }

        public decimal DailyAmountUpperLimit { get; private set; }

        [Column(StringLength = 100)]
        public string TimezoneName { get; private set; }

        public int AvailableTimeRangeStartHour { get; private set; }

        public int AvailableTimeRangeEndHour { get; private set; }
    }
}
