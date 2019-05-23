using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LaksaCsharp.Utils
{
    public class Bech32
    {
        /**
         * The Bech32 character set for encoding.
         */
        private static string CHARSET = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";

        /**
         * The Bech32 character set for decoding.
         */
        private static sbyte[] CHARSET_REV = {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            15, -1, 10, 17, 21, 20, 26, 30, 7, 5, -1, -1, -1, -1, -1, -1,
            -1, 29, -1, 24, 13, 25, 9, 8, 23, -1, 18, 22, 31, 27, 19, -1,
            1, 0, 3, 16, 11, 28, 12, 14, 6, 4, 2, -1, -1, -1, -1, -1,
            -1, 29, -1, 24, 13, 25, 9, 8, 23, -1, 18, 22, 31, 27, 19, -1,
            1, 0, 3, 16, 11, 28, 12, 14, 6, 4, 2, -1, -1, -1, -1, -1 };

        public class Bech32Data
        {
            public string hrp;
            public byte[] data;

            public Bech32Data(string hrp, byte[] data)
            {
                this.hrp = hrp;
                this.data = data;
            }
        }

        public static int RightMove(int value, int pos)
        {
            //移动 0 位时直接返回原值
            if (pos != 0)
            {
                // int.MaxValue = 0x7FFFFFFF 整数最大值
                int mask = int.MaxValue;
                //无符号整数最高位不表示正负但操作数还是有符号的，有符号数右移1位，正数时高位补0，负数时高位补1
                value = value >> 1;
                //和整数最大值进行逻辑与运算，运算后的结果为忽略表示正负值的最高位
                value = value & mask;
                //逻辑运算后的值无符号，对无符号的值直接做右移运算，计算剩下的位
                value = value >> pos - 1;
            }

            return value;
        }

        /**
         * Find the polynomial with value coefficients mod the generator as 30-bit.
         */
        private static int Polymod(byte[] values)
        {
            int c = 1;
            foreach (byte v_i in values)
            {
                int c0 = (RightMove(c, 25)) & 0xff;
                c = ((c & 0x1ffffff) << 5) ^ (v_i & 0xff);
                if ((c0 & 1) != 0) c ^= 0x3b6a57b2;
                if ((c0 & 2) != 0) c ^= 0x26508e6d;
                if ((c0 & 4) != 0) c ^= 0x1ea119fa;
                if ((c0 & 8) != 0) c ^= 0x3d4233dd;
                if ((c0 & 16) != 0) c ^= 0x2a1462b3;
            }
            return c;
        }

        /**
         * Expand a HRP for use in checksum computation.
         */
        private static byte[] ExpandHrp(string hrp)
        {
            int hrpLength = hrp.Length;
            byte[] ret = new byte[hrpLength * 2 + 1];
            for (int i = 0; i < hrpLength; ++i)
            {
                int c = hrp.ToCharArray()[i] & 0x7f; // Limit to standard 7-bit ASCII
                ret[i] = (byte)((RightMove(c, 5)) & 0x07);
                ret[i + hrpLength + 1] = (byte)(c & 0x1f);
            }
            ret[hrpLength] = 0;
            return ret;
        }

        /**
         * Verify a checksum.
         */
        private static bool VerifyChecksum(string hrp, byte[] values)
        {
            byte[] hrpExpanded = ExpandHrp(hrp);
            byte[] combined = new byte[hrpExpanded.Length + values.Length];
            Array.Copy(hrpExpanded, 0, combined, 0, hrpExpanded.Length);
            Array.Copy(values, 0, combined, hrpExpanded.Length, values.Length);
            return Polymod(combined) == 1;
        }

        /**
         * Create a checksum.
         */
        private static byte[] CreateChecksum(string hrp, byte[] values)
        {
            byte[] hrpExpanded = ExpandHrp(hrp);
            byte[] enc = new byte[hrpExpanded.Length + values.Length + 6];
            Array.Copy(hrpExpanded, 0, enc, 0, hrpExpanded.Length);
            Array.Copy(values, 0, enc, hrpExpanded.Length, values.Length);
            int mod = Polymod(enc) ^ 1;
            byte[] ret = new byte[6];
            for (int i = 0; i < 6; ++i)
            {
                ret[i] = (byte)((RightMove(mod, (5 * (5 - i)))) & 31);
            }
            return ret;
        }

        /**
         * Encode a Bech32 string.
         */
        public static string Encode(Bech32Data bech32)
        {
            return Encode(bech32.hrp, bech32.data);
        }

        /**
         * Encode a Bech32 string.
         */
        public static string Encode(string hrp, byte[] values)
        {
            if (hrp.Length < 1)
            {
                throw new Exception("Human-readable part is too short");
            }
            if (hrp.Length > 83)
            {
                throw new Exception("Human-readable part is too long");
            }
            hrp = hrp.ToLower(CultureInfo.CurrentCulture);
            byte[] checksum = CreateChecksum(hrp, values);
            byte[] combined = new byte[values.Length + checksum.Length];
            Array.Copy(values, 0, combined, 0, values.Length);
            Array.Copy(checksum, 0, combined, values.Length, checksum.Length);
            StringBuilder sb = new StringBuilder(hrp.Length + 1 + combined.Length);
            sb.Append(hrp);
            sb.Append('1');
            foreach (byte b in combined)
            {
                sb.Append(CHARSET.ToCharArray()[b]);
            }
            return sb.ToString();
        }

        /**
         * Decode a Bech32 string.
         */
        public static Bech32Data Decode(string str)
        {
            bool lower = false, upper = false;
            if (str.Length < 8)
                throw new InvalidDataLength("Input too short: " + str.Length);
            if (str.Length > 90)
                throw new InvalidDataLength("Input too long: " + str.Length);
            for (int i = 0; i < str.Length; ++i)
            {
                char c = str.ToCharArray()[i];
                if (c < 33 || c > 126) throw new InvalidCharacter(c, i);
                if (c >= 'a' && c <= 'z')
                {
                    if (upper)
                        throw new InvalidCharacter(c, i);
                    lower = true;
                }
                if (c >= 'A' && c <= 'Z')
                {
                    if (lower)
                        throw new InvalidCharacter(c, i);
                    upper = true;
                }
            }
            int pos = str.LastIndexOf('1');
            if (pos < 1) throw new InvalidPrefix("Missing human-readable part");
            int dataPartLength = str.Length - 1 - pos;
            if (dataPartLength < 6)
                throw new InvalidDataLength("Data part too short: " + dataPartLength);
            byte[] values = new byte[dataPartLength];
            for (int i = 0; i < dataPartLength; ++i)
            {
                char c = str.ToCharArray()[(i + pos + 1)];
                if (CHARSET_REV[c] == -1) throw new InvalidCharacter(c, i + pos + 1);
                values[i] = (byte)CHARSET_REV[c];
            }
            string hrp = str.Substring(0, pos).ToLower(CultureInfo.CurrentCulture);
            if (!VerifyChecksum(hrp, values)) throw new InvalidChecksum();
            byte[] newValues = new byte[values.Length - 6];
            Array.Copy(values, newValues, values.Length - 6);
            return new Bech32Data(hrp, newValues);
        }


        public static string HRP = "zil";


        public static List<int> ConvertBits(byte[] data, int fromWidth, int toWidth, bool pad)
        {
            int acc = 0;
            int bits = 0;
            int maxv = (1 << toWidth) - 1;
            List<int> ret = new List<int>();

            for (int i = 0; i < data.Length; i++)
            {
                int value = data[i] & 0xff;
                if (value < 0 || value >> fromWidth != 0)
                {
                    return null;
                }
                acc = (acc << fromWidth) | value;
                bits += fromWidth;
                while (bits >= toWidth)
                {
                    bits -= toWidth;
                    ret.Add((acc >> bits) & maxv);
                }
            }

            if (pad)
            {
                if (bits > 0)
                {
                    ret.Add((acc << (toWidth - bits)) & maxv);
                }
                else if (bits >= fromWidth || ((acc << (toWidth - bits)) & maxv) != 0)
                {
                    return null;
                }
            }

            return ret;

        }

        public static string ToBech32Address(string address)
        {
            if (!Validation.IsAddress(address))
            {
                throw new Exception("Invalid address format.");
            }

            address = address.ToLower().Replace("0x", "");

            List<int> bits = ConvertBits(ByteUtil.HexStringToByteArray(address), 8, 5, false);

            byte[]
        addrBz = new byte[bits.Count];

            for (int i = 0; i < bits.Count; i++)
            {
                addrBz[i] = BitConverter.GetBytes(bits[i])[0];
            }

            if (null == addrBz)
            {
                throw new Exception("Could not convert byte Buffer to 5-bit Buffer");
            }

            return Encode(HRP, addrBz);
        }

        public static string FromBech32Address(string address)
        {
            Bech32Data data = Decode(address);

            if (data.hrp != HRP)
            {
                throw new Exception("Expected hrp to be zil");
            }

            List<int> bits = ConvertBits(data.data, 5, 8, false);
            byte[]
        buf = new byte[bits.Count];
            for (int i = 0; i < bits.Count; i++)
            {
                buf[i] = BitConverter.GetBytes(bits[i])[0];
            }
            if (null == buf || buf.Length == 0)
            {
                throw new Exception("Could not convert buffer to bytes");
            }

            return Account.Account.ToCheckSumAddress(ByteUtil.ByteArrayToHexString(buf)).Replace("0x", "");
        }


    }
}
