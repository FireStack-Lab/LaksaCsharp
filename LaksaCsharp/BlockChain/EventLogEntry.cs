using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.BlockChain
{
    public class EventLogEntry
    {
        public string Address { get; set; }
        public string EventName { get; set; }
        public EventParam[] Params { get; set; }
    }

}
