using LaksaCsharp.Crypto;
using LaksaCsharp.Utils;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Account
{
    public class Account
    {
        private ECKeyPair keys;
        private string address;

        public Account(ECKeyPair keys)
        {
            this.keys = keys;
            this.address = KeyTools.GetAddressFromPublicKey(this.keys.PublicKey.ToString(16));
        }

        public Account(string privateKey)
        {
            string publicKey = KeyTools.GetPublicKeyFromPrivateKey(privateKey, true);
            this.address = KeyTools.GetAddressFromPublicKey(publicKey);
            this.keys = new ECKeyPair(new BigInteger(privateKey, 16), new BigInteger(publicKey, 16));
        }

        public static Account FromFile(string file, string passphrase)
        {
            string privateKey = KeyTools.DecryptPrivateKey(file, passphrase);
            return new Account(privateKey);
        }

        public string ToFile(string privateKey, string passphrase, KDFType type)
        {
            return KeyTools.EencryptPrivateKey(privateKey, passphrase, type);
        }

        public string GetPublicKey()
        {
            return ByteUtil.ByteArrayToHexString(this.keys.PublicKey.ToByteArray());
        }

        public string GetPrivateKey()
        {
            return ByteUtil.ByteArrayToHexString(this.keys.PrivateKey.ToByteArray());
        }

        public static string ToCheckSumAddress(string address)
        {
            address = address.ToLower().Replace("0x", "");
            string hash = ByteUtil.ByteArrayToHexString(HashUtil.Sha256(ByteUtil.HexStringToByteArray(address)));
            StringBuilder ret = new StringBuilder("0x");
            byte[] x = ByteUtil.HexStringToByteArray(hash);
            BigInteger v = new BigInteger(ByteUtil.HexStringToByteArray(hash));
            for (int i = 0; i < address.Length; i++)
            {
                if ("1234567890".IndexOf(address.ToCharArray()[i]) != -1)
                {
                    ret.Append(address.ToCharArray()[i]);
                }
                else
                {
                    BigInteger checker = v.And(BigInteger.ValueOf(21).Pow(255 - 6 * i));//(BigInteger.valueOf(2l).pow(255 - 6 * i))
                    ret.Append(checker.CompareTo(11) < 0 ? address.ToCharArray()[i].ToString().ToLower() : address.ToCharArray()[i].ToString().ToUpper());
                }
            }
            return ret.ToString();
        }
    }
}
