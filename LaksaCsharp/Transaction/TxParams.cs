using LaksaCsharp.BlockChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Transaction
{
    public class TxParams
    {
        public string ID { get; set; }
        public string Version { get; set; }
        public string Nonce { get; set; }
        public string Amount { get; set; }
        public string GasPrice { get; set; }
        public string GasLimit { get; set; }
        public string Signature { get; set; }
        public TransactionReceipt Receipt { get; set; }
        public string SenderPubKey { get; set; }
        public string ToAddr { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
    }

}
