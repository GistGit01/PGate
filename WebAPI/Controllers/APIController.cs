using Base.Repositories;
using Base.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Orders;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class APIController : APIBaseController
    {
        private readonly CreateOrderService _createOrderService;
        private readonly RefundService _refundService;
        private readonly IOrderRepository _orderRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IConfiguration _configuration;

        public APIController(CreateOrderService createOrderService, RefundService refundService, IOrderRepository orderRepository, IChannelRepository channelRepository, IConfiguration configuration)
        {
            _createOrderService = createOrderService;
            _refundService = refundService;
            _orderRepository = orderRepository;
            _channelRepository = channelRepository;
            _configuration = configuration;
        }

        [APIActionFilter]
        [Route("createOrder")]
        [HttpPost]
        public async Task<CreateOrderResult> CreateOrderAsync([FromBody]CreateOrderRequest request)
        {
            var channel = await _channelRepository.Where(p => p.Name == request.Channel).FirstAsync();

            var order = await _createOrderService.CreateOrderAsync(request.CustomerId.Value, request.PlatformEnum, channel.Id, request.Currency, request.Amount, request.OutOrderId, request.Narrative, _configuration["NotifyUrl"], request.NotifyUrl, request.RedirectUrl);

            return new CreateOrderResult
            {
                OrderId = order.Id,
                PayUrl = order.PayUrl
            };
        }

        [APIActionFilter]
        [Route("refund")]
        [HttpPost]
        public async Task<RefundResult> RefundAsync([FromBody]RefundRequest request)
        {
            var refund = await _refundService.RefundAsync(request.CustomerId.Value, request.OrderId, request.RefundAmount);

            return new RefundResult
            {
                RefundId = refund.Id
            };
        }

        [APIActionFilter]
        [Route("queryOrder")]
        [HttpPost]
        public async Task<QueryOrderResult> QueryOrderAsync([FromBody] QueryOrderRequest request)
        {
            var order = await _orderRepository.FindAsync(request.OrderId);
            if (order == null || order?.CustomerId != request.CustomerId)
                throw new Exception("订单不存在");

            return new QueryOrderResult
            {
                OrderId = order.Id,
                Status = order.Status,
                PayTime = order.PayTime
            };
        }
    }
}
