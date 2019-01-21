using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Crypto
{
    public class PBKDF2Params : KDFParams
    {
        public string Salt { get; set; }
        public int DkLen { get; set; }
        public int Count { get; set; }
    }

}
