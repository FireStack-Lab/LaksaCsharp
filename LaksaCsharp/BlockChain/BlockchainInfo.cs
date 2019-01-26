using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.BlockChain
{
    public class BlockchainInfo
    {
        public int NumPeers { get; set; }
        public string NumTxBlocks { get; set; }
        public string NumDSBlocks { get; set; }
        public string NumTransactions { get; set; }
        public string TransactionRate { get; set; }
        public double TxBlockRate { get; set; }
        public double DSBlockRate { get; set; }
        public string CurrentMiniEpoch { get; set; }
        public string CurrentDSEpoch { get; set; }
        public string NumTxnsDSEpoch { get; set; }
        public int NumTxnsTxEpoch { get; set; }
        public ShardingStructure ShardingStructure { get; set; }
    }
}
