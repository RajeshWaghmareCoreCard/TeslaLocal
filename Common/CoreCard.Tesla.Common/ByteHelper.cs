using System;
using System.Linq;
using System.Text;

namespace CoreCard.Tesla.Common
{
    public class ByteHelper
    {

        public static byte[] CombineByteArrays(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }
        public static byte[] GetBitmapFromHexString(string bitmapString)
        {

            byte[] bitmap = new byte[8];
            int count = 0;
            for (int i = 0; i < bitmapString.Length; i += 2)
            {
                bitmap[count++] = Convert.ToByte(bitmapString.Substring(i, 2), 16);
            }

            return bitmap;
        }

        public static byte[] GetBytesFromHexString(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length / 2; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(2 * i, 2), 16);
            }

            return bytes;
        }

        public static string GetHexRepresentationOfByteData(byte[] bytes)
        {
            var hexString = new StringBuilder();
            foreach (byte b in bytes)
                hexString.AppendFormat("{0:X2}", b);

            return hexString.ToString();
        }

        public static byte[] ParseHex(string hex)
        {
            int offset = hex.StartsWith("0x") ? 2 : 0;
            if ((hex.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid length: " + hex.Length);
            }
            byte[] ret = new byte[(hex.Length - offset) / 2];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)((ParseNybble(hex[offset]) << 4)
                                 | ParseNybble(hex[offset + 1]));
                offset += 2;
            }
            return ret;
        }

        private static int ParseNybble(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }
            throw new ArgumentException("Invalid hex digit: " + c);
        }

    }
}
