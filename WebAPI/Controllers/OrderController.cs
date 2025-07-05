using Base.Repositories;
using Base.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Orders;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly CreateOrderService _createOrderService;
        private readonly IChannelRepository _channelRepository;
        private readonly IConfiguration _configuration;

        public OrderController(CreateOrderService createOrderService, IChannelRepository channelRepository, IConfiguration configuration)
        {
            _createOrderService = createOrderService;
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
    }
}
