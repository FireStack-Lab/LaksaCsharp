using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LaksaCsharp.Jsonrpc
{
    public class Req<T>
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public T[] Params { get; set; }
    }
}
