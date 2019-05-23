using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaksaCsharp.Utils
{
    public class Base58
    {
        public static char[] ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz".ToCharArray();
        private static char ENCODED_ZERO = ALPHABET[0];
        private static int[] INDEXES = new int[128];

        static Base58()
        {
            for (int i = 0; i < INDEXES.Length; i++)
            {
                INDEXES[i] = -1;
            }

            for (int i = 0; i < ALPHABET.Length; i++)
            {
                INDEXES[ALPHABET[i]] = i;
            }
        }

        public static string Encode(string input)
        {
            if (input.ToUpper().StartsWith("0X"))
            {
                input = input.Substring(2);
            }
            return Encode(ByteUtil.HexStringToByteArray(input));
        }


        public static string Encode(byte[] input)
        {
            if (input.Length == 0)
            {
                return "";
            }
            // Count leading zeros.
            int zeros = 0;
            while (zeros < input.Length && input[zeros] == 0)
            {
                ++zeros;
            }
            // Convert base-256 digits to base-58 digits (plus conversion to ASCII characters)
            // since we modify it in-place
            byte[] newInput = new byte[input.Length];
            Array.Copy(input, newInput, input.Length);
            input = newInput;
            char[] encoded = new char[input.Length * 2]; // upper bound
            int outputStart = encoded.Length;
            for (int inputStart = zeros; inputStart < input.Length;)
            {
                encoded[--outputStart] = ALPHABET[Divmod(input, inputStart, 256, 58)];
                if (input[inputStart] == 0)
                {
                    ++inputStart; // optimization - skip leading zeros
                }
            }
            // Preserve exactly as many leading encoded zeros in output as there were leading zeros in input.
            while (outputStart < encoded.Length && encoded[outputStart] == ENCODED_ZERO)
            {
                ++outputStart;
            }
            while (--zeros >= 0)
            {
                encoded[--outputStart] = ENCODED_ZERO;
            }
            // Return encoded string (including encoded leading zeros).
            return new string(encoded, outputStart, encoded.Length - outputStart);
        }


        public static byte[] Decode(string input)
        {
            if (input.Length == 0)
            {
                return new byte[0];
            }
            // Convert the base58-encoded ASCII chars to a base58 byte sequence (base58 digits).
            byte[]
        input58 = new byte[input.Length];
            for (int i = 0; i < input.Length; ++i)
            {
                char c = input.ToCharArray()[i];
                int digit = c < 128 ? INDEXES[c] : -1;
                if (digit < 0)
                {
                    throw new Exception("invalid character");
                }
                input58[i] = (byte)digit;
            }
            // Count leading zeros.
            int zeros = 0;
            while (zeros < input58.Length && input58[zeros] == 0)
            {
                ++zeros;
            }
            // Convert base-58 digits to base-256 digits.
            byte[] decoded = new byte[input.Length];
            int outputStart = decoded.Length;
            for (int inputStart = zeros; inputStart < input58.Length;)
            {
                decoded[--outputStart] = Divmod(input58, inputStart, 58, 256);
                if (input58[inputStart] == 0)
                {
                    ++inputStart; // optimization - skip leading zeros
                }
            }
            // Ignore extra leading zeroes that were added during the calculation.
            while (outputStart < decoded.Length && decoded[outputStart] == 0)
            {
                ++outputStart;
            }
            // Return decoded data (including original number of leading zeros).
            byte[] result = new byte[decoded.Length - outputStart + zeros];
            Array.Copy(decoded, outputStart - zeros, result, 0, decoded.Length);
            return result;
        }

        public static BigInteger DecodeToBigInteger(string input)
        {
            return new BigInteger(1, Decode(input));
        }

        private static byte Divmod(byte[] number, int firstDigit, int bases, int divisor)
        {
            // this is just long division which accounts for the base of the input digits
            int remainder = 0;
            for (int i = firstDigit; i < number.Length; i++)
            {
                int digit = (int)number[i] & 0xFF;
                int temp = remainder * bases + digit;
                number[i] = (byte)(temp / divisor);
                remainder = temp % divisor;
            }
            return (byte)remainder;
        }

        public static bool IsBase58(string v)
        {
            try
            {
                Base58.Decode(v);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
