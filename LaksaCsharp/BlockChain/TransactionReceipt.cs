using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.BlockChain
{
    public class TransactionReceipt
    {
        public bool Success { get; set; }
        public string Cumulative_gas { get; set; }
        public string Epoch_num { get; set; }
    }

}
