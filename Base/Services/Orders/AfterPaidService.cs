using Base.Models;
using Base.Repositories;
using Base.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Orders
{
    public class AfterPaidService : IScoped
    {
        private readonly IChannelMerchantOrderSummaryRepository _summaryRepository;

        public AfterPaidService(IChannelMerchantOrderSummaryRepository summaryRepository)
        {
            _summaryRepository = summaryRepository;
        }

        public void AfterPaid(Order order, PaymentRule rule)
        {
            _summaryRepository.Orm.Transaction(() =>
            {
                // 更新订单状态
                order.Paid(DateTime.UtcNow);

                order.Update();

                // 记录统计信息

                var record = _summaryRepository.Where(p => p.ChannelMerchantId == order.ChannelMerchantId && p.SummaryDate == DateTime.Now.GetCurrentDateInSpecificTimezone(rule.TimezoneId)).First();

                var recordDateTime = DateTime.UtcNow.ConvertToSpecificTimezone(rule.TimezoneId);

                if (record == null)
                    record = new ChannelMerchantOrderSummary(order.ChannelMerchantId, 1, order.Amount, recordDateTime, recordDateTime.GetCurrentDateInSpecificTimezone(rule.TimezoneId), order.CustomerId);
                else
                    record.RecordOrder(order.Amount, recordDateTime);

                record.Save();
            });
        }
    }
}
