using Base.Models;
using Base.Repositories;
using FreeSql;

namespace FSqlRepositories
{
    public class PaymentRuleRepository : BaseRepository<PaymentRule, long>, IPaymentRuleRepository
    {
        public PaymentRuleRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
