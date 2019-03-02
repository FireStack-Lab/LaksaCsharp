using Google.Protobuf.WellKnownTypes;
using LaksaCsharp.Account;
using LaksaCsharp.Crypto;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Utils;
using Org.BouncyCastle.Crypto.Digests;
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
        public Wallet Signer { get; set; }
        public HttpProvider Provider { get; set; }

        public static string GetAddressForContract(Transaction.Transaction tx)
        {
            string senderAddress = KeyTools.GetAddressFromPublicKey(tx.SenderPubKey);
            SHA256Managed.Create().ComputeHash(ByteUtil.HexStringToByteArray(senderAddress));
            Sha256Digest sha = new Sha256Digest();
            byte[] senderAddressBytes = ByteUtil.HexStringToByteArray(senderAddress);
            sha.BlockUpdate(senderAddressBytes, 0, senderAddressBytes.Count());

            int nonce = 0;
            if (!string.IsNullOrEmpty(tx.Nonce))
            {
                nonce = int.Parse(tx.Nonce);
                nonce--;
            }
            string hexNonce = Validation.IntToHex(nonce, 16);
            byte[] hexNonceBytes = ByteUtil.HexStringToByteArray(hexNonce);
            sha.BlockUpdate(hexNonceBytes, 0, hexNonceBytes.Count());
            byte[] bytes = new byte[sha.GetByteLength()];
            sha.DoFinal(bytes, 0);

            return ByteUtil.ByteArrayToHexString(bytes).Substring(24, 40);
        }

        public Contract NewContract(string code, Values[] init, string abi)
        {
            return new Contract(this, code, abi, null, init, null);
        }

        public Contract AtContract(String address, String code, Values[] init, String abi)
        {
            return new Contract(this, code, abi, address, init, null);
        }
    }
}
