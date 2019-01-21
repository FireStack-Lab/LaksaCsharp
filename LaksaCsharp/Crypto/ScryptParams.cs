using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Crypto
{
    public class ScryptParams : KDFParams
    {
        public string Salt { get; set; }
        public int DkLen { get; set; }
        public int N { get; set; }
        public int R { get; set; }
        public int P { get; set; }
    }

}
