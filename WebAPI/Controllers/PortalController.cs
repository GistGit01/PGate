using Base.Repositories;
using Base.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Models;
using WebAPI.Models.Portal;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Route("/portal/[controller]")]
    public class PortalController : PortalBaseController
    {
        private readonly ICustomerRepository _customerRepository;

        public PortalController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [PortalActionFilter(IsAnonymous = true)]
        [Route("signin")]
        [HttpPost]
        public async Task<SigninResult> SigninAsync([FromBody] SigninRequest request, [FromServices] IMemoryCache cache)
        {
            var encryptedPassword = SignHelper.MakeSign(request.Password);

            var customer = await _customerRepository.Where(p => p.Email == request.Email && p.Password == encryptedPassword).FirstAsync();
            if (customer == null)
                throw new Exception("客户不存在或密码错误");

            var token = Guid.NewGuid().ToString("N");

            cache.Set(token, customer);

            return new SigninResult
            {
                Token = token
            };
        }

        [PortalActionFilter]
        [Route("signout")]
        [HttpPost]
        public ResultModel Signout([FromBody] PortalRequestModel request, [FromServices] IMemoryCache cache)
        {
            cache.Remove(request.Token);

            return new ResultModel();
        }

        [PortalActionFilter]
        [Route("getAccounts")]
        [HttpPost]
        public async Task<GetAccountsResult> GetAccountsAsync([FromBody]GetAccountsRequest request, [FromServices]IAccountRepository accountRepository)
        {
            var accounts = await accountRepository.Where(p => p.CustomerId == Customer.Id).ToListAsync();
            return new GetAccountsResult
            {
                Accounts = accounts
            };
        }

        //[PortalActionFilter]
        //[Route("getAccountTransactions")]
        //[HttpPost]
        //public async Task<GetAccountTransactionsResult> GetAccountTransactionsAsync([FromBody] GetAccountTransactionsRequest request, [FromServices]ITransactionRepository transactionRepository)
        //{
            
        //}
    }
}
