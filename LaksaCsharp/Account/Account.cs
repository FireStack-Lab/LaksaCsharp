using LaksaCsharp.Crypto;
using LaksaCsharp.Utils;
using OC.Core.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Account
{
    public class Account
    {
        private OC.Core.Crypto.ECKeyPair keys;
        private string address;

        public Account(OC.Core.Crypto.ECKeyPair keys)
        {
            this.keys = keys;
            this.address = KeyTools.GetAddressFromPublicKey(this.keys.PublicKey);
        }

        public Account(String privateKey)
        {
            string publicKey = KeyTools.GetPublicKeyFromPrivateKey(privateKey, true);
            this.address = KeyTools.GetAddressFromPublicKey(publicKey);
            this.keys = new OC.Core.Crypto.ECKeyPair(int.Parse(privateKey));//TODO
        }

        public static Account FromFile(String file, String passphrase)
        {
            string privateKey = KeyTools.DecryptPrivateKey(file, passphrase);
            return new Account(privateKey);
        }

        public string ToFile(String privateKey, String passphrase, KDFType type)
        {
            return KeyTools.EencryptPrivateKey(privateKey, passphrase, type);
        }

        public string GetPublicKey()
        {
            return ByteUtil.ByteArrayToHexString(Encoding.Default.GetBytes(this.keys.PublicKey));
        }

        public string GetPrivateKey()
        {
            return ByteUtil.ByteArrayToHexString(Encoding.Default.GetBytes(this.keys.PrivateKey));
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
                    BigInteger checker = v & BigInteger.Pow(21, 255 - 6 * i);//(BigInteger.valueOf(2l).pow(255 - 6 * i))
                    ret.Append(checker.CompareTo(11) < 0 ? address.ToCharArray()[i].ToString().ToLower() : address.ToCharArray()[i].ToString().ToUpper());
                }
            }
            return ret.ToString();
        }
    }
}
