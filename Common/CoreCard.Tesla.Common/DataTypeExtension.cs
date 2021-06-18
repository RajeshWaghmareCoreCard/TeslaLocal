using System;

namespace CoreCard.Tesla.Common
{
    public static class DataTypeExtension
    {
        public static int TryToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
            }
            return 0;
        }
        public static bool TryToBool(this object obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
            }
            return false;
        }
        public static int TryToBit(this bool obj)
        {
            try
            {
                if (obj) return 1;
                return 0;
            }
            catch
            {
            }
            return 0;
        }
        public static long TryToLong(this object obj)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
            }
            return 0;
        }
    }
}