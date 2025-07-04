using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public interface IRequestModel
    {

    }

    public abstract class RequestModelBase : IRequestModel
    {
        public RequestModelBase()
        {

        }

        /// <summary>
        /// 商户ID
        /// </summary>
        [Display(Name = "客户ID")]
        [JsonPropertyName("customerId")]
        public long? CustomerId { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [Display(Name = "签名")]
        [JsonPropertyName("sign")]
        public string? Sign { get; set; } = null!;

        /// <summary>
        /// 时间戳
        /// </summary>
        [Display(Name = "时间戳")]
        [JsonPropertyName("timestamp")]
        public long? TimeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        [Display(Name = "随机字符串")]
        [JsonPropertyName("nonce")]
        public string? NonceString { get; set; } = null!;
    }

    public sealed class RequestModel : RequestModelBase
    {

    }

    public interface IResult
    {
    }

    public class ResultModel : IResult
    {
        public ResultModel()
        {
            ReturnCode = "SUCCESS";
        }

        public ResultModel(string failMessage, string errorCode)
        {
            ReturnCode = "FAIL";
            //ErrorCode = errorCode;
            FailMessage = failMessage;
        }

        public static ResultModel Fail(string message, string code)
        {
            return new ResultModel
            {
                //ErrorCode = code,
                FailMessage = message,
                ReturnCode = "FAIL"
            };
        }

        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("returnCode")]
        public string ReturnCode { get; private set; } = "SUCCESS";

        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success => ReturnCode != "FAIL";

        /// <summary>
        /// 失败消息
        /// </summary>
        [JsonPropertyName("failMessage")]
        public string? FailMessage { get; private set; }

        ///// <summary>
        ///// 错误代码
        ///// </summary>
        //[JsonPropertyName("errorCode")]
        //public string? ErrorCode { get; private set; }
    }
}
