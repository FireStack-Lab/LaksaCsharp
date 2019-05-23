using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Utils
{
    public class HashUtil
    {

        public static byte[] Sha256(byte[] bytes)
        {
            SHA256 sha = new SHA256Managed();
            return sha.ComputeHash(bytes);
        }

        public static byte[] Sha3(byte[] bytes)
        {
            Sha256Digest digest = new Sha256Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return result;
        }

        public static byte[] GenerateMac(byte[] derivedKey, byte[] cipherText)
        {
            byte[] result = new byte[16 + cipherText.Length];

            Array.Copy(derivedKey, 16, result, 0, 16);
            Array.Copy(cipherText, 0, result, 16, cipherText.Length);

            return HashUtil.Sha256(result);
        }
    }
}
