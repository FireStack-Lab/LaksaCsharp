using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using LaksaCsharp.Utils;

namespace LaksaTest.Utils
{
    /// <summary>
    /// ByteUtilTest 的摘要说明
    /// </summary>
    [TestFixture]
    public class ByteUtilTest
    {
        [Test]
        public void HexStringToByteArray()
        {
            String hexString = "e19d05c5452598e24caad4a0d85a49146f7be089515c905ae6a19e8a578a6930";
            String byteString = "225,157,5,197,69,37,152,226,76,170,212,160,216,90,73,20,111,123,224,137,81,92,144,90,230,161,158,138,87,138,105,48,";
            byte[] bytes = ByteUtil.HexStringToByteArray(hexString);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b & 0xff);
                stringBuilder.Append(",");
            }
            System.Console.WriteLine(stringBuilder.ToString());
            Assert.AreEqual(stringBuilder.ToString(), byteString);
        }
    }
}
