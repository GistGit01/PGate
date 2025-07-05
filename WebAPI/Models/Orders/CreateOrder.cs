using Base.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Orders
{
    public class CreateOrderRequest : RequestModelBase
    {
        [Display(Name ="平台")]
        [Required(ErrorMessage = "{0}为必传参数")]
        public string Platform { get; set; }

        [Display(Name = "通道")]
        [Required(ErrorMessage = "{0}为必传参数")]
        public string Channel { get; set; }

        [Display(Name = "货币")]
        [Required(ErrorMessage = "{0}为必传参数")]
        public string Currency { get; set; }

        [Display(Name = "金额")]
        [Required(ErrorMessage = "{0}为必传参数")]
        public decimal Amount { get; set; }

        [Display(Name = "外部订单ID")]
        [Required(ErrorMessage = "{0}为必传参数")]
        public string OutOrderId { get; set; }

        [Display(Name = "订单描述")]
        public string? Narrative { get; set; }

        [Display(Name = "异步通知地址")]
        public string? NotifyUrl { get; set; }

        [Display(Name = "前台跳转地址")]
        public string? RedirectUrl { get; set; }

        public PaymentPlatform PlatformEnum
        {
            get
            {
                if (Enum.TryParse<PaymentPlatform>(Platform, out PaymentPlatform result))
                    return result;
                else
                    throw new Exception("平台不支持");
            }
        }
    }

    public class CreateOrderResult :ResultModel
    {
        public long OrderId { get; set; }

        public string PayUrl { get; set; }
    }
}
