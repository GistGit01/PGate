using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    [Table(Name="channel_merchant_order_summary")]
    public class ChannelMerchantOrderSummary :Entity<ChannelMerchantOrderSummary>
    {
        public ChannelMerchantOrderSummary(long channelMerchantId, int dailyOrderCount, decimal dailyAmountSummary, DateTime lastOrderDateTime, DateTime summaryDate, long customerId)
        {
            ChannelMerchantId = channelMerchantId;
            DailyOrderCount = dailyOrderCount;
            DailyAmountSummary = dailyAmountSummary;
            LastOrderDateTime = lastOrderDateTime;
            SummaryDate = summaryDate;
            CustomerId = customerId;
        }

        private ChannelMerchantOrderSummary() {}

        public void RecordOrder(decimal amount, DateTime payTime)
        {
            this.LastOrderDateTime = payTime;
            this.DailyAmountSummary += amount;
        }

        public long CustomerId { get; private set; }

        public long ChannelMerchantId { get; private set; }

        public int DailyOrderCount { get; private set; }

        public decimal DailyAmountSummary { get; private set; }

        public DateTime LastOrderDateTime { get; private set; }

        public DateTime SummaryDate { get; private set; }
    }
}
