using System;
using Newtonsoft.Json;

namespace CoreCard.Tesla.Common
{
    public static class StringExtensions
    {
        public static string MaskPan(this string pan)
        {
            if (string.IsNullOrEmpty(pan) || pan.Length <= 4)
                return pan;

            if (pan.Length > 10)
                return pan.Substring(0, 6) + new string('X', pan.Length - 10) + pan.Substring(pan.Length - 4);

            if (pan.Length > 4)
                return new string('X', pan.Length - 4) + pan.Substring(pan.Length - 4);

            return new string('X', pan.Length);
        }

        public static string MaskStringData(this string dataStringToMask)
        {
            return string.IsNullOrEmpty(dataStringToMask) ? dataStringToMask : new string('X', dataStringToMask.Length);
        }

        public static string MaskTrack2Data(this string track2Data)
        {
            if (string.IsNullOrEmpty(track2Data) || track2Data.Length <= 4)
                return track2Data;

            if (track2Data.Length > 4)
                return new string('X', track2Data.Length - 4) + track2Data.Substring(track2Data.Length - 4);

            return new string('X', track2Data.Length);

        }

        public static byte[] TryFromBase64String(this string base64String)
        {
            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch
            {
                return null;
            }
        }


        public static T TryFromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
            }
            return default(T);
        }
        public static string TryToBase64String(this byte[] byteArray)
        {
            try
            {
                return Convert.ToBase64String(byteArray);
            }
            catch
            {
                return null;
            }
        }
        public static double TryToDouble(this string value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }


        public static int TryToInt32(this string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        public static string TryToJson(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return null;
            }
        }
        public static double TryToMoney(this string value)
        {
            try
            {
                var amount = Convert.ToDouble(value);
                if (amount > 0)
                    return amount = amount * 100;
                return amount;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal TryToDecimal(this string value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }
        public static bool IsNullOrEmpty(this string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                    return true;
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
