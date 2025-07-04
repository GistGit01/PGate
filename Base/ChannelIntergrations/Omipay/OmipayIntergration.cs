using Base.Models;
using Base.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Util;
using Flurl.Http;
using Google.Protobuf.WellKnownTypes;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using Flurl;
using Base.Utils;

namespace Base.ChannelIntergrations.Omipay
{
    public class OmipayIntergration : IChannelIntergration
    {
        private const string BASE_URL = @"https://www.omipay.com.cn/omipay/api/v2";

        private readonly ChannelMerchant _merchant;
        private readonly string _notifyUrl;

        #region Respnose Models

        private class OmipayResponseModel
        {
            public string return_code { get; set; }

            public bool success => return_code == "SUCCESS";

            public string? error_code { get; set; }

            public string? error_msg { get; set; }

            public virtual T SolveError<T>() where T: OmipayResponseModel
            {
                if (!success)
                    throw new Exception($"[{error_code}]: {error_msg}");
                return (T)this;
            }
        }

        private class CreateOrderResponseModel : OmipayResponseModel
        {
            public string order_no { get; set; }
            public string pay_url { get; set; }
        }

        private class QueryOrderResponseModel : OmipayResponseModel
        {
            public string out_order_no { get; set; }

            public string currency { get; set; }

            public int amount { get; set; }

            public decimal amountInDecimal => amount.ToCurrencyDecimal(currency);

            public string pay_currency { get; set; }

            public int pay_amount { get; set; }

            public decimal payAmountInDecimal => pay_amount.ToCurrencyDecimal(pay_currency);

            public DateTime order_time { get; set; }

            public DateTime? pay_time { get; set; }

            public int exchange_rate { get; set; }

            public decimal ExchangeRateInDecimal => (Decimal)exchange_rate / 100000000;

            public string result_code { get; set; }

            public OrderStatus Status
            {
                get
                {
                    switch (result_code)
                    {
                        case "READY": return OrderStatus.SubmittedToChannel;
                        case "PAYING": return OrderStatus.SubmittedToChannel;
                        case "PAID": return OrderStatus.Paid;
                        case "CLOSED": return OrderStatus.Settled;
                        case "CANCELLED": return OrderStatus.CANCELLED;
                        case "FAILED": return OrderStatus.FAILED;
                        default: throw new Exception("状态错误");
                    }
                }
            }
        }

        private class RefundResponseModel:OmipayResponseModel
        {
            public string refund_no { get; set; }

            public string currency { get; set; }

            public int amount { get; set; }

            public decimal AmountInDecimal => amount.ToCurrencyDecimal(currency);

            public DateTime refund_time { get; set; }
        }

        private class QueryRefundResponseModel : OmipayResponseModel
        {
            public string result_code { get; set; }

            public string out_refund_no { get; set; }

            public string currency { get; set; }

            public int amount { get; set; }

            public decimal AmountInDecimal => amount.ToCurrencyDecimal(currency);

            public DateTime refund_time { get; set; }

            public DateTime? success_time { get; set; }

            public RefundStatus Status
            {
                get
                {
                    switch (return_code)
                    {
                        case "Applied": return RefundStatus.Created;
                        case "Paymentchannelconfirmed": return RefundStatus.Refunded;
                        case "Cleared": return RefundStatus.Settled;
                        case "CustomerCancelled": return RefundStatus.Cancelled;
                        case "OrganizationFailed": return RefundStatus.Failed;
                        default: return RefundStatus.Created;
                    }
                }
            }
        }

        private class ExchangeRateResponseModel : OmipayResponseModel
        {
            public decimal rate { get; set; }
        }

        #endregion

        public OmipayIntergration(ChannelMerchant merchant, string notifyUrl)
        {
            _merchant = merchant;
            _notifyUrl = notifyUrl;
        }

        #region APIs

        public async Task<decimal> GetExchangeRateAsync(PaymentPlatform platform, string originCurrency, string targetCurrency)
        {
            string platformStr = "";
            if (platform == PaymentPlatform.WechatPay)
                platformStr = "WECHATPAY";
            else if (platform == PaymentPlatform.Alipay)
                platformStr = "ALIPAY";
            else
                throw new Exception("Platform not supported");

            var parameters = new Dictionary<string, object>
            {
                { "currency", originCurrency },
                { "base_currency", targetCurrency },
                { "platform", platformStr }
            };

            var url = GenerateUrl("GetExchangeRate", parameters);
            var response = await url.PostAsync();
            var rateResponseModel = await response.GetJsonAsync<ExchangeRateResponseModel>();
            return rateResponseModel.SolveError<ExchangeRateResponseModel>().rate;
        }

        public async Task<(string channelOrderId, string payUrl)> CreateOrderAsync(PaymentPlatform platform, string currency, decimal amount, string orderId, string narrative, string? redirectUrl)
        {
            switch(platform)
            {
                case PaymentPlatform.WechatPay:
                    return await CreateJSAPIOrderAsync(currency, amount, orderId, narrative, redirectUrl);
                case PaymentPlatform.Alipay:
                case PaymentPlatform.AlipayPlus:
                    return await CreateOnlineOrderAsync(platform, currency, amount, orderId, narrative, redirectUrl);
                default:
                    throw new Exception("不支持的支付平台");
            }
        }

        private async Task<(string channelOrderId, string payUrl)> CreateJSAPIOrderAsync(string currency, decimal amount, string orderId, string narrative, string? redirectUrl)
        {
            var parameters = new Dictionary<string, object>
            {
                { "order_name", narrative },
                { "currency", currency },
                { "amount", amount.ToCurrencyInt(currency) },
                { "notify_url", UrlEncoder.Default.Encode(_notifyUrl) },
                { "redirect_url", redirectUrl??"" },
                { "out_order_no", orderId }
            };

            var url = GenerateUrl("MakeJSAPIOrder", parameters);
            var response = await url.PostAsync();
            var orderResponseModel = await response.GetJsonAsync<CreateOrderResponseModel>();
            orderResponseModel.SolveError<CreateOrderResponseModel>();
            return (channelOrderId: orderResponseModel.order_no, payUrl: orderResponseModel.pay_url);
        }

        private async Task<(string channelOrderId, string payUrl)> CreateOnlineOrderAsync(PaymentPlatform platform, string currency, decimal amount, string orderId, string narrative, string? redirectUrl)
        {
            var platformStr = "ALIPAYONLINE";
            switch (platform)
            {
                case PaymentPlatform.WechatPay:
                    platformStr = "WECHATPAY";
                    break;
                case PaymentPlatform.Alipay:
                    platformStr = "ALIPAYONLINE";
                    break;
                case PaymentPlatform.AlipayPlus:
                    platformStr = "ALIPAYPLUS";
                    break;
                default:
                    throw new Exception("Not support");
            }

            var parameters = new Dictionary<string, object>
            {
                { "order_name", narrative },
                { "currency", currency },
                { "amount", amount.ToCurrencyInt(currency) },
                { "notify_url", UrlEncoder.Default.Encode(_notifyUrl) },
                { "return_url", redirectUrl??"" },
                { "out_order_no", orderId },
                { "type", "web" },
                { "platform", platformStr }
            };

            var url = GenerateUrl("MakeOnlineOrder", parameters);
            var response = await url.PostAsync();
            var orderResponseModel = await response.GetJsonAsync<CreateOrderResponseModel>();
            orderResponseModel.SolveError<CreateOrderResponseModel>();
            return (channelOrderId: orderResponseModel.order_no, payUrl: orderResponseModel.pay_url);
        }

        public async Task<(OrderStatus status, DateTime? payTime)> QueryOrderAsync(string channelOrderId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "order_no", channelOrderId }
            };

            var url = GenerateUrl("QueryOrder", parameters);
            var response = await url.PostAsync();
            var queryResponseModel = await response.GetJsonAsync<QueryOrderResponseModel>();
            queryResponseModel.SolveError<QueryOrderResponseModel>();
            return (queryResponseModel.Status, queryResponseModel.pay_time);
        }

        public async Task<RefundStatus> QueryRefundAsync(string channelRefundId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "refund_no", channelRefundId }
            };
            var url = GenerateUrl("QueryRefund", parameters);
            var response = await url.PostAsync();
            var queryResponseModel = await response.GetJsonAsync<QueryRefundResponseModel>();
            queryResponseModel.SolveError<QueryRefundResponseModel>();
            return queryResponseModel.Status;
        }

        public async Task<(string channelRefundId, DateTime refundTime)> RefundOrderAsync(string channelOrderId, decimal amount, string currency)
        {
            var parameters = new Dictionary<string, object>
            {
                { "order_no", channelOrderId },
                { "amount", amount.ToCurrencyInt(currency) }
            };

            var url = GenerateUrl("Refund", parameters);
            var response = await url.PostAsync();
            var refundResponseModel = await response.GetJsonAsync<RefundResponseModel>();
            refundResponseModel.SolveError<RefundResponseModel>();
            return (channelRefundId: refundResponseModel.refund_no, refundTime: refundResponseModel.refund_time);
        }

        #endregion

        private (string nonceStr, long timeStamp, string sign) GenerateSignParams()
        {
            var timeStamp = Convert.ToInt64((DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds);
            var nonceString = Guid.NewGuid().ToString("N");
            var originString = $"{_merchant.ChannelMerchantNumber}&{timeStamp}&{nonceString}&{_merchant.ChannelSecretKey}";

            var bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(originString));
            var signBuilder = new StringBuilder();
            foreach (byte b in bytes)
                signBuilder.Append(b.ToString("x2"));
            var signString = signBuilder.ToString().ToUpper();

            return (nonceString, timeStamp, signString);
        }

        private string GenerateUrl(string endpointName, Dictionary<string, object> parameters)
        {
            var signParams = GenerateSignParams();

            parameters.Add("m_number", _merchant.ChannelMerchantNumber);
            parameters.Add("timestamp", signParams.timeStamp);
            parameters.Add("nonce_str", signParams.nonceStr);
            parameters.Add("sign", signParams.sign);

            var paramList = parameters.Select(p => $"{p.Key}={p.Value}");
            var queryString = paramList.Aggregate((x, y) => $"{x}&{y}");

            var url = $@"{BASE_URL}/{endpointName}?{queryString}";

            return url;
        }
    }
}
