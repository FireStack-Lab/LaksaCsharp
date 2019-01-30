using LaksaCsharp.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaksaCsharp.Proto;
using Google.Protobuf;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Math;

namespace LaksaCsharp.Utils
{
    public class TransactionUtil
    {

        public byte[] EncodeTransactionProto(TxParams txParams)
        {
            BigInteger amount = BigInteger.ValueOf(long.Parse(txParams.Amount));
            BigInteger gasPrice = BigInteger.ValueOf(long.Parse(txParams.GasPrice));

            ProtoTransactionCoreInfo info = new ProtoTransactionCoreInfo();
            info.Version = uint.Parse(txParams.Version);
            info.Nonce = (string.IsNullOrEmpty(txParams.Nonce) ? 0L : ulong.Parse(txParams.Nonce));
            info.Toaddr = ByteString.CopyFrom(ByteUtil.HexStringToByteArray(txParams.ToAddr.ToLower()));
            info.Senderpubkey = new ByteArray() { Data = ByteString.CopyFrom(ByteUtil.HexStringToByteArray(txParams.SenderPubKey)) };
            info.Amount = new ByteArray() { Data = ByteString.CopyFrom(BigIntegers.AsUnsignedByteArray(16, amount)) };
            info.Gasprice = new ByteArray() { Data = ByteString.CopyFrom(BigIntegers.AsUnsignedByteArray(16, gasPrice)) };
            info.Gaslimit = ulong.Parse(txParams.GasLimit);
            if (!string.IsNullOrEmpty(txParams.Code))
            {
                info.Code = ByteString.CopyFrom(System.Text.Encoding.Default.GetBytes(txParams.Code));
            }

            if (!string.IsNullOrEmpty(txParams.Data))
            {
                info.Data = ByteString.CopyFrom(System.Text.Encoding.Default.GetBytes(txParams.Data));
            }


            return info.ToByteArray();
        }

    }
}
