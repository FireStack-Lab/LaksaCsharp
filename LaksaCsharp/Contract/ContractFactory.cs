using Google.Protobuf.WellKnownTypes;
using LaksaCsharp.Account;
using LaksaCsharp.Crypto;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Contract
{
    public class ContractFactory
    {
        private Wallet signer;
        private HttpProvider provider;

        public HttpProvider Provider
        {
            get { return provider; }
        }

        public Wallet Signer
        {
            get { return signer; }
        }

        public static string GetAddressForContract(Transaction.Transaction tx)
        {
            string senderAddress = KeyTools.GetAddressFromPublicKey(tx.SenderPubKey);
            SHA256Managed.Create().ComputeHash(ByteUtil.HexStringToByteArray(senderAddress));

            int nonce = 0;
            if (!string.IsNullOrEmpty(tx.Nonce))
            {
                nonce = int.Parse(tx.Nonce);
                nonce--;
            }
            string hexNonce = Validation.IntToHex(nonce, 16);

            byte[] bytes = SHA256Managed.Create().ComputeHash(ByteUtil.HexStringToByteArray(hexNonce));

            return ByteUtil.ByteArrayToHexString(bytes).Substring(24);
        }

        public Contract NewContract(string code, Value[] init, string abi)
        {
            return new Contract(this, code, abi, null, init, null);
        }

        public Contract AtContract(String address, String code, Value[] init, String abi)
        {
            return new Contract(this, code, abi, address, init, null);
        }
    }
}
