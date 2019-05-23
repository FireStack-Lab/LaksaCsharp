using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Utils
{
    public class ByteUtil
    {
        private static char[] hexArray = "0123456789ABCDEF".ToCharArray();

        public static string ByteArrayToHexString(sbyte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 3);
            foreach (byte b in bytes)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToUpper();
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 3);
            foreach (byte b in bytes)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToUpper().Replace(" ", "");
        }

        public static sbyte[] ToSbyte(byte[] bytes)
        {
            sbyte[] sbytes = new sbyte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] > 127)
                    sbytes[i] = (sbyte)(bytes[i] - 256);
                else
                    sbytes[i] = (sbyte)bytes[i];
            }

            return sbytes;
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "").ToLower().Replace("0x", "");
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return buffer;

        }
    }
}
