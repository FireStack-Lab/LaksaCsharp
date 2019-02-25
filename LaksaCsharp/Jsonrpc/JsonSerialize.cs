using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Jsonrpc
{
    public class JsonSerialize : RestSharp.Serializers.ISerializer
    {
        public string ContentType
        {
            get
            {
                return "application/json";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
