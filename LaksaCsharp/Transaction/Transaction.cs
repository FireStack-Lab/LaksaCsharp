using LaksaCsharp.BlockChain;
using LaksaCsharp.Jsonrpc;
using LaksaCsharp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LaksaCsharp.Transaction
{
    public class Transaction
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("gasPrice")]
        public string GasPrice { get; set; }
        [JsonProperty("gasLimit")]
        public string GasLimit { get; set; }
        [JsonProperty("signature")]
        public string Signature { get; set; }
        public TransactionReceipt Receipt { get; set; }
        [JsonProperty("pubKey")]
        public string SenderPubKey { get; set; }
        [JsonProperty("toAddr")]
        public string ToAddr { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }

        public HttpProvider Provider { get; set; }
        public TxStatus Status { get; set; }

        public TxParams ToTransactionParam()
        {
            return new TxParams()
            {
                ID = this.ID,
                Version = this.Version,
                Nonce = this.Nonce,
                Amount = this.Amount,
                GasPrice = this.GasPrice,
                GasLimit = this.GasLimit,
                Signature = this.Signature,
                Receipt = this.Receipt,
                SenderPubKey = this.SenderPubKey.ToLower(),
                ToAddr = this.ToAddr == null ? "0000000000000000000000000000000000000000" : this.ToAddr.ToLower(),
                Code = this.Code,
                Data = this.Data,
            };
        }

        public TransactionPayload ToTransactionPayload()
        {
            return new TransactionPayload()
            {
                Version = int.Parse(this.Version),
                Nonce = int.Parse(this.Nonce),
                Amount = this.Amount,
                PubKey = this.SenderPubKey.ToLower(),
                GasPrice = this.GasPrice,
                GasLimit = this.GasLimit,
                Signature = this.Signature,
                Code = this.Code,
                Data = this.Data,
                ToAddr = Account.Account.NormaliseAddress(this.ToAddr),
                Priority = false,
            };
        }

        public void MarshalToAddress()
        {
            byte[] address = Base58.Decode(this.ToAddr);
            this.ToAddr = ByteUtil.ByteArrayToHexString(address);
        }
        public byte[] Bytes()
        {
            TxParams txParams = ToTransactionParam();
            TransactionUtil util = new TransactionUtil();
            byte[] bytes = util.EncodeTransactionProto(txParams);
            return bytes;
        }

        public bool IsPending()
        {
            return this.Status == TxStatus.Pending;
        }

        public bool IsInitialised()
        {
            return this.Status == TxStatus.Initialised;
        }

        public bool IsConfirmed()
        {
            return this.Status == TxStatus.Confirmed;
        }

        public bool IsRejected()
        {
            return this.Status == TxStatus.Rejected;
        }

        public Transaction Confirm(String txHash, int maxAttempts, int interval)
        {
            this.Status = TxStatus.Pending;
            for (int i = 0; i < maxAttempts; i++)
            {
                bool tracked = this.TrackTx(txHash);
                Thread.Sleep(interval * 1000);

                if (tracked)
                {
                    this.Status = TxStatus.Confirmed;
                    return this;
                }
            }
            this.Status = TxStatus.Rejected;
            return this;
        }

        public bool TrackTx(string txHash)
        {
            Transaction response;
            try
            {
                response = this.Provider.GetTransaction(txHash).Result;
            }
            catch (IOException e)
            {
                Console.WriteLine("transaction not confirmed yet");
                return false;
            }

            if (null == response)
            {
                Console.WriteLine("transaction not confirmed yet");
                return false;
            }


            this.ID = response.ID;
            this.Receipt = response.Receipt;

            if (response.Receipt != null && response.Receipt.Success)
            {
                Console.WriteLine("Transaction confirmed!");
                this.Status = TxStatus.Confirmed;
            }
            else
            {
                this.Status = TxStatus.Rejected;
                Console.WriteLine("Transaction rejected!");

            }
            return true;
        }


    }

}
