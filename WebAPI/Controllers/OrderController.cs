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

        public OrderController(CreateOrderService createOrderService)
        {
            _createOrderService = createOrderService;
        }

        [APIActionFilter]
        [Route("create_order")]
        [HttpPost]
        public async Task<CreateOrderResult> CreateOrderAsync([FromBody]CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
