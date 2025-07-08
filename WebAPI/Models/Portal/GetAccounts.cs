using Base.Models;

namespace WebAPI.Models.Portal
{
    public class GetAccountsRequest: PortalRequestModelBase
    {

    }

    public class GetAccountsResult : ResultModel
    {
        public List<Account> Accounts { get; set; } = [];
    }
}
