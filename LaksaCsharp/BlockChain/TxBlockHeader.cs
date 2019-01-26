using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.BlockChain
{
    public class TxBlockHeader
    {
        public string BlockNum { get; set; }
        public string DSBlockNum { get; set; }
        public string GasLimit { get; set; }
        public string GasUsed { get; set; }
        public string MbInfoHash { get; set; }
        public string MinerPubKey { get; set; }
        public int NumMicroBlocks { get; set; }
        public int NumTxns { get; set; }
        public string PrevBlockHash { get; set; }
        public string Rewards { get; set; }
        public string StateDeltaHash { get; set; }
        public string StateRootHash { get; set; }
        public string Timestamp { get; set; }
        public int Type { get; set; }
        public int Version { get; set; }
    }

}
