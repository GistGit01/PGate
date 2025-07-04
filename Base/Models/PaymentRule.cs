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
        public PaymentRule(long channelId,
                           string channelName,
                           int merchantPaymentFrequencyLowerLimit,
                           string currency,
                           decimal orderAmountUpperLimit,
                           decimal dailyAmountUpperLimit,
                           int dailyCountUpperLimitForMerchant,
                           int dailyCountUpperLimitForCustomer,
                           string timezoneId,
                           int availableTimeRangeStartHour,
                           int availableTimeRangeEndHour)
        {
            ChannelId = channelId;
            ChannelName = channelName;
            MerchantPaymentFrequencyLowerLimit = merchantPaymentFrequencyLowerLimit;
            Currency = currency;
            OrderAmountUpperLimit = orderAmountUpperLimit;
            DailyAmountUpperLimit = dailyAmountUpperLimit;
            DailyCountUpperLimitForMerchant = dailyCountUpperLimitForMerchant;
            DailyCountUpperLimitForCustomer = dailyCountUpperLimitForCustomer;
            TimezoneId = timezoneId;
            AvailableTimeRangeStartHour = availableTimeRangeStartHour;
            AvailableTimeRangeEndHour = availableTimeRangeEndHour;
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

        public int DailyCountUpperLimitForMerchant { get; private set; }

        public int DailyCountUpperLimitForCustomer { get; private set; }

        [Column(StringLength = 100)]
        public string TimezoneId { get; private set; }

        public int AvailableTimeRangeStartHour { get; private set; }

        public int AvailableTimeRangeEndHour { get; private set; }
    }
}
