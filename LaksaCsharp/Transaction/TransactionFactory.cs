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
        public static Transaction buildTransaction(TxParams param, HttpProvider provider, TxStatus status)
        {
            return Transaction.builder()
                    .ID(params.getID())
                    .version(params.getVersion())
                    .nonce(params.getNonce())
                    .amount(params.getAmount())
                    .gasPrice(params.getGasPrice())
                    .gasLimit(params.getGasLimit())
                    .signature(params.getSignature())
                    .receipt(params.getReceipt())
                    .senderPubKey(params.getSenderPubKey())
                    .toAddr(params.getToAddr())
                    .code(params.getCode())
                    .data(params.getData())
                    .provider(provider)
                    .status(status)
                    .build();
        }

        public static HttpProvider.CreateTxResult sendTransaction(Transaction signedTx)
        {
        return signedTx.getProvider().createTransaction(signedTx.toTransactionPayload());
        }
    }

}
