using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Utils
{
    public static class CurrencyExtensions
    {
        private static Dictionary<string, int> _dictCurrencyDecimals = new Dictionary<string, int>
        {
            { "AUD", 2 },
            { "CNY", 2 },
            { "USD", 2 }
        };

        public static decimal ToCurrencyDecimal(this int amount, string currency)
        {
            var decimals = _dictCurrencyDecimals[currency];
            
            return Convert.ToDecimal(amount) / Convert.ToDecimal(Math.Pow(10, -decimals));
        }

        public static int ToCurrencyInt(this decimal amount, string currency)
        {
            var decimals = _dictCurrencyDecimals[currency];

            return Convert.ToInt32(Math.Round(amount * Convert.ToDecimal(Math.Pow(10, -decimals))));
        }
    }
}
