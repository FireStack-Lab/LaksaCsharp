using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.BlockChain
{
    public class DsBlockHeader
    {
        public string BlockNum { get; set; }
        public int Difficulty { get; set; }
        public int DiffcultyDS { get; set; }
        public int GasPrice { get; set; }
        public string LeaderPublicKey { get; set; }
        public string[] PowWinners { get; set; }
        public string PrevHash { get; set; }
        public string Timestamp { get; set; }
    }
}
