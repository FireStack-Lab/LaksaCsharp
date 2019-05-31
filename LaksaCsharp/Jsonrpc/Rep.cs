using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Jsonrpc
{
    public class Rep<T>
    {
        public string Id { get; set; }
        public string Jsonrpc { get; set; }
        public T Result { get; set; }
        public string Err { get; set; }
    }

}
