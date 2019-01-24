using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LaksaCsharp.Utils;
using LaksaCsharp.Account;

namespace LaksaTest.Utils
{
    /// <summary>
    /// ByteUtilTest 的摘要说明
    /// </summary>
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void ToCheckSumAddress()
        {
            Assert.AreEqual(Account.ToCheckSumAddress("4BAF5FADA8E5DB92C3D3242618C5B47133AE003C"), "0x4BAF5faDA8e5Db92C3d3242618c5B47133AE003C");
        }
    }
}
