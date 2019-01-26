using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.BlockChain
{
    public class Contract
    {
        public string Address { get; set; }
        public State[] State { get; set; }
      
    }

    public class State
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Vname { get; set; }
    }
}
