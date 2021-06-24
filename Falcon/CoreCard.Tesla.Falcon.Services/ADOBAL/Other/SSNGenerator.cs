using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class SSNGenerator
    {
        public static string RandomSSN(string thePrefix, string delimiter = "")
        {
            string generatedSSN = GenerateSSN(delimiter);
            return thePrefix + generatedSSN.Substring(thePrefix.Length);
        }

        public static string GenerateSSN(string delimiter="-")
        {
            int iThree = GetRandomNumber(132, 921);
            int iTwo = GetRandomNumber(12, 83);
            int iFour = GetRandomNumber(1423, 9211);
            return iThree.ToString() + delimiter + iTwo.ToString() + delimiter + iFour.ToString();
        }

        //Function to get random number
        private static readonly Random getrandom = new Random();
        public static int GetRandomNumber(int min, int max)
        {
            return getrandom.Next(min, max);
        }
    }
}
