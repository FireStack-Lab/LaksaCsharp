using LaksaCsharp.Jsonrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Transaction
{
    public class TransactionFactory
    {
        public static Transaction BuildTransaction(TxParams param, HttpProvider provider, TxStatus status)
        {
            return new Transaction()
            {
                ID = param.ID,
                Version = param.Version,
                Nonce = param.Nonce,
                Amount = param.Amount,
                GasPrice = param.GasPrice,
                GasLimit = param.GasLimit,
                Signature = param.Signature,
                Receipt = param.Receipt,
                SenderPubKey = param.SenderPubKey,
                ToAddr = param.ToAddr,
                Code = param.Code,
                Data = param.Data,
                Provider = provider,
                Status = status,
            };
        }

        public static CreateTxResult CreateTransaction(Transaction signedTx)
        {
            return signedTx.Provider.CreateTransaction(signedTx.ToTransactionPayload()).Result;
        }
    }

}
