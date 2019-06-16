using LaksaCsharp.BlockChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.BlockChain
{
    public class TxBlock
    {
        public Body Body { get; set; }
        public TxBlockHeader Header { get; set; }
    }

    public class MicroBlockInfo
    {
        public string MicroBlockHash { get; set; }
        public int MicroBlockShardId { get; set; }
        public string MicroBlockTxnRootHash { get; set; }
    }

    public class Body
    {
        public string BlockHash { get; set; }
        public string HeaderSign { get; set; }
        public MicroBlockInfo[] MicroBlockInfos { get; set; }
    }

}
