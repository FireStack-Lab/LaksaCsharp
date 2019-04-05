using LaksaCsharp.Utils;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Crypto
{
    public class Signature
    {
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }


        public override string ToString()
        {
            return ByteUtil.ByteArrayToHexString(R.ToByteArray()) + ByteUtil.ByteArrayToHexString(S.ToByteArray());
        }
    }
}
