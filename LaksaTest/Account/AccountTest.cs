﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LaksaCsharp.Utils;
using LaksaCsharp.Account;

namespace LaksaTest.Account
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
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("4BAF5FADA8E5DB92C3D3242618C5B47133AE003C"), "0x4BAF5faDA8e5Db92C3d3242618c5B47133AE003C");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("448261915A80CDE9BDE7C7A791685200D3A0BF4E"), "0x448261915a80cdE9BDE7C7a791685200D3A0bf4E");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("DED02FD979FC2E55C0243BD2F52DF022C40ADA1E"), "0xDed02fD979fC2e55c0243bd2F52df022c40ADa1E");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("13F06E60297BEA6A3C402F6F64C416A6B31E586E"), "0x13F06E60297bea6A3c402F6f64c416A6b31e586e");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("1A90C25307C3CC71958A83FA213A2362D859CF33"), "0x1a90C25307C3Cc71958A83fa213A2362D859CF33");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("625ABAEBD87DAE9AB128F3B3AE99688813D9C5DF"), "0x625ABAebd87daE9ab128f3B3AE99688813d9C5dF");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("36BA34097F861191C48C839C9B1A8B5912F583CF"), "0x36Ba34097f861191C48C839c9b1a8B5912f583cF");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("D2453AE76C9A86AAE544FCA699DBDC5C576AEF3A"), "0xD2453Ae76C9A86AAe544fca699DbDC5c576aEf3A");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("72220E84947C36118CDBC580454DFAA3B918CD97"), "0x72220e84947c36118cDbC580454DFaa3b918cD97");
            Assert.AreEqual(LaksaCsharp.Account.Account.ToCheckSumAddress("50F92304C892D94A385CA6CE6CD6950CE9A36839"), "0x50f92304c892D94A385cA6cE6CD6950ce9A36839");
        }
    }
}
