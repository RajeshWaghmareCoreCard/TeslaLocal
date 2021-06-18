using System;

namespace CoreCard.Tesla.Common
{
    public static class HelperExtensions
    {
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString("D");
        }
        public static string GetGUIDWithoutDash()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }
    }
}