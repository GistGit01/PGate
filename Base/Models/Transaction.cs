using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    public class Transaction:Entity<Transaction>
    {
        private Transaction() {}


        public long AccountId { get; private set; }

        public long ChannelId { get; private set; }

        [Column(StringLength = 20)]
        public string ChannelName { get; private set; }

        public decimal Amount { get; private set; }

        public decimal BeforeBalance { get; private set; }

        public decimal AfterBalance => BeforeBalance + Amount;

        [Column(StringLength = 10)]
        public string Currency { get; private set; }

        [Column(StringLength = 200)]
        public string Description { get; private set; }
    }
}
