using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Transaction
{
    public class TransactionPayload
    {
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("nonce")]
        public int Nonce { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("gasPrice")]
        public string GasPrice { get; set; }
        [JsonProperty("gasLimit")]
        public string GasLimit { get; set; }
        [JsonProperty("signature")]
        public string Signature { get; set; }
        [JsonProperty("pubKey")]
        public string PubKey { get; set; }
        [JsonProperty("toAddr")]
        public string ToAddr { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("priority")]
        public bool Priority { get; set; }
    }
}
