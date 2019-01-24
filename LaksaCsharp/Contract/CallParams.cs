using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Contract
{
    public class CallParams
    {
        public string ID { get; set; }
        public string Version { get; set; }
        public string Nonce { get; set; }
        public string Amount { get; set; }
        public string GasPrice { get; set; }
        public string GasLimit { get; set; }
        public string SenderPubKey { get; set; }
    }

}
