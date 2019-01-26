using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Transaction
{
    public class TransactionPayload
    {
        public int Version { get; set; }
        public int Nonce { get; set; }
        public string ToAddr { get; set; }
        public string Amount { get; set; }
        public string PubKey { get; set; }
        public string GasPrice { get; set; }
        public string GasLimit { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public string Signature { get; set; }
    }
}
