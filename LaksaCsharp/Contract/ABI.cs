using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Contract
{
    public class ABI
    {
        public string Name { get; set; }
        public Field[] Fields { get; set; }
        public Field[] Params { get; set; }
        public Transition[] Transitions { get; set; }
    }

}
