namespace WebAPI.Models.API
{
    public class RefundRequest : APIRequestModelBase
    {
        public decimal RefundAmount { get; set; }

        public long OrderId { get; set; }
    }

    public class RefundResult : ResultModel
    {
        public long RefundId { get; set; }
    }
}
