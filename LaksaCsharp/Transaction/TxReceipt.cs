using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Transaction
{
    public class TxReceipt
    {
        public bool Success { get; set; }
        public int CumulativeGas { get; set; }
    }
}
