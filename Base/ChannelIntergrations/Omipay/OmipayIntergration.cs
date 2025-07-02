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

namespace Base.ChannelIntergrations.Omipay
{
    public class OmipayIntergration : IChannelIntergration
    {
        private const string BASE_URL = @"https://www.omipay.com.cn/omipay/api/v2";

        private readonly Merchant _merchant;
        private readonly string _notifyUrl;

        public async Task<Order> CreateOrderAsync(PaymentPlatform platform, string currency, decimal amount, string orderId, string narrative, string? redirectUrl)
        {
            switch(platform)
            {
                case PaymentPlatform.WechatPay:
                    break;
                case PaymentPlatform.Alipay:
                    break;
                default:
                    throw new Exception("Not support");
            }

            var parameters = new Dictionary<string, object>
            {
                { "order_name", narrative },
                { "currency", currency },
                { "amount", amount },
                { "notify_url", UrlEncoder.Default.Encode(_notifyUrl) },
                { "redirect_url", redirectUrl??"" },
                { "out_order_no", orderId }
            };

            var url = GenerateUrl("MakeJSAPIOrder", parameters);
            var response = await url.PostAsync();
            var orderResponseModel = await response.GetJsonAsync<CreateJSAPIOrderResponseModel>();

            return new Order(_merchant.ChannelId, _merchant.ChannelName, orderResponseModel.order_no, "", currency, amount, narrative, orderResponseModel.pay_url, redirectUrl);
        }

        private class OmipayResponseModel
        {
            public string return_code { get; set; }

            public bool success => return_code == "SUCCESS";            
        }

        private class CreateJSAPIOrderResponseModel : OmipayResponseModel
        {
            public string order_no { get; set; }
            public string pay_url { get; set; }
        }

        public OrderStatus QueryOrder(string channelOrderId)
        {
            throw new NotImplementedException();
        }

        public RefundStatus QueryRefund(string channelRefundId)
        {
            throw new NotImplementedException();
        }

        public Refund RefundOrder(string channelOrderId, decimal amount, string refundId)
        {
            throw new NotImplementedException();
        }

        public OmipayIntergration(Merchant merchant, string notifyUrl)
        {
            _merchant = merchant;
            _notifyUrl = notifyUrl;
        }

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
