﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LaksaCsharp.Utils;

namespace LaksaTest.Utils
{
    /// <summary>
    /// ByteUtilTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void IsByteString()
        {
            bool result = Validation.IsByteString("e9c49caf0d0bc9d7c769391e8bda2028f824cf3d", 40);
            Assert.IsTrue(result);
            result = Validation.IsByteString("e9c49caf0d0bc9d7c769391e8bda2028f824cf3", 40);
            Assert.IsFalse(result);
            result = Validation.IsByteString("e9c49caf0d0bc9d7c76g391e8bda2028f824cf3d", 40);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidChecksumAddress()
        {
            Assert.IsTrue(Validation.IsValidChecksumAddress("0x4BAF5faDA8e5Db92C3d3242618c5B47133AE003C"));
            Assert.IsTrue(Validation.IsValidChecksumAddress("0x4BAF5FaDA8e5Db92C3d3242618c5B47133AE003C"));
        }
    }
}
