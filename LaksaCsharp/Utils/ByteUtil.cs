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

        public static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 3);
            foreach (byte b in bytes)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToUpper();
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return buffer;

        }
    }
}
