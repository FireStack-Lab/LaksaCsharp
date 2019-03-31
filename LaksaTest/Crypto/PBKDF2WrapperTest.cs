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
    public class PBKDF2WrapperTest
    {
        [Test]
        public void GenerateDerivedScryptKey()
        {
            PBKDF2Wrapper pbkdf2Wrapper = new PBKDF2Wrapper();
            PBKDF2Params param = new PBKDF2Params();
            param.Salt = "0f2274f6c0daf36d5822d97985be5a3d881d11e2e741bad4e038a099eecc3b6d";
            param.Count = 262144;
            param.DkLen = 32;
            byte[] bytes = pbkdf2Wrapper.GetDerivedKey(System.Text.Encoding.Default.GetBytes("stronk_password"),
                    ByteUtil.HexStringToByteArray(param.Salt), param.Count, param.DkLen);
            byte[] macArray = HashUtil.GenerateMac(bytes, ByteUtil.HexStringToByteArray("dc55047d51f795509ffb6969db837a4481887ccfb6bfb7c259fb77b19078c2a4"));
            Console.WriteLine(ByteUtil.ByteArrayToHexString(macArray));
            Assert.AreEqual(ByteUtil.ByteArrayToHexString(macArray).ToLower(), "dedc361c53c421974c2811f7f989bc530aebf9a90c487b4161e0e54ae6faba31");

        }
    }
}
