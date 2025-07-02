using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    [Table(Name ="account")]
    public class Account:Entity<Account>
    {
        public Account(string number, long channelId, string channelName, decimal balance, string currency)
        {
            Number = number;
            ChannelId = channelId;
            ChannelName = channelName;
            Balance = balance;
            Currency = currency;
        }

        private Account() {}

        public string Number { get; private set; }

        public long ChannelId { get; private set; }

        public string ChannelName { get; private set; }

        public decimal Balance { get; private set; }

        public string Currency { get; private set; }

        public void SetBalance(decimal amount)
        {
            Balance = amount;
        }

        public void AddBalance(decimal amount)
        {
            Balance += amount;
        }

        public void DeductBalance(decimal amount)
        {
            Balance -= amount;
        }
    }
}
