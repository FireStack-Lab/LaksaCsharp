using LaksaCsharp.Crypto;
using LaksaCsharp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaTest.Crypto
{
    [TestFixture]
    public class ScryptWrapperTest
    {
        [Test]
        public void GenerateDerivedScryptKey()
        {
            ScryptWrapper scryptWrapper = new ScryptWrapper();
            ScryptParams param = new ScryptParams();
            param.DkLen = 32;
            param.Salt = "2c37db13a633c5a5e5b8c699109690e33860b7eb43bbc81bbab47d4e9c29f1b9";
            param.N = 8192;
            param.R = 8;
            param.P = 1;

            byte[] bytes = scryptWrapper.GetDerivedKey(System.Text.Encoding.Default.GetBytes("stronk_password"), ByteUtil.HexStringToByteArray(param.Salt), param.N, param.R, param.P, param.DkLen);
            byte[] macArray = HashUtil.GenerateMac(bytes, ByteUtil.HexStringToByteArray("ecdf81453d031ac2fa068b7185ddac044fa4632d3b061400d3c07a86510b4823"));
            Console.WriteLine(ByteUtil.ByteArrayToHexString(macArray));
            Assert.AreEqual("ed7fa37a4adbc8b7bbe0d43a329a047f89e2dcf7f2dfc96babfe79edd955f7a3", ByteUtil.ByteArrayToHexString(macArray).ToLower());
        }
    }
}
