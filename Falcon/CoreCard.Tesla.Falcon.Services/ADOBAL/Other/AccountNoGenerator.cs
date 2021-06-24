using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public static class AccountNoGenerator
    {
        public static Int64 RandomAccountNumber()
        {
            Random RandNum = new Random();

            //return RandNum.Next(1000000000, 9999999999);

            var builder = new StringBuilder();
            while (builder.Length < 10)
            {
                builder.Append(RandNum.Next(1, 10).ToString());
            }

            return Convert.ToInt64(builder.ToString());
        }

        public static string RandomCardNumber()
        {

            Random RandNum = new Random();
            var builder = new StringBuilder();
            while (builder.Length < (16))
            {
                builder.Append(RandNum.Next(1, 10).ToString());
                //if (builder.Length % 4 == 0)
                //{
                //    builder.Append("-");
                //}
            }
            return builder.ToString();
        }

    }
}
