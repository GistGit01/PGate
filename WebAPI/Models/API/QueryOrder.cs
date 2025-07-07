using Base.Models.Enums;

namespace WebAPI.Models.API
{
    public class QueryOrderRequest : APIRequestModelBase
    {
        public long OrderId { get; set; }
    }

    public class QueryOrderResult : ResultModel
    {
        public long OrderId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime? PayTime { get; set; }
    }
}
