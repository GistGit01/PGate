using Base.ChannelIntergrations.Omipay;

namespace TEST
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var omi = new OmipayIntergration(new Base.Models.ChannelMerchant("RR Surfer", 1, "OMIPAY", "c6c86d21d2c140a08e2b41c789aec5ae", "000034"), "https://www.test.com");
            var order = omi.CreateOrderAsync(Base.Models.Enums.PaymentPlatform.WechatPay, "AUD", 1, "123123", "TEST", "").Result;
        }
    }
}