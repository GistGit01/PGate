using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Base.Utils
{
    public static class SignHelper
    {
        public static bool VerifySign(long customerId, long timeStamp, string nonceString, string secretKey, string sign)
        {
            var originString = $"{customerId}&{timeStamp}&{nonceString}&{secretKey}";
            var localSign = MakeSign(originString);

            if (localSign != sign)
                return false;

            return true;
        }

        public static string Sign(long customerId, long timeStamp, string nonceString, string secretKey)
        {
            var originString = $"{customerId}&{timeStamp}&{nonceString}&{secretKey}";
            var sign = MakeSign(originString);
            return sign;
        }

        private static string MakeSign(string originString)
        {
            var sha256 = SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(originString);
            var hashBytes = sha256.ComputeHash(bytes);
            var sb = new StringBuilder();

            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            var sign = sb.ToString().ToUpper();

            return sign;
        }
    }
}
