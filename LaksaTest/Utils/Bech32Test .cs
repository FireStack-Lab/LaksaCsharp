using LaksaCsharp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaksaTest.Utils
{
    [TestFixture]
    public class Bech32Test
    {
        [Test]
        public void ToBech32AddressTest()
        {
            string bech32 = Bech32.ToBech32Address("0x9bfec715a6bd658fcb62b0f8cc9bfa2ade71434a");
            Assert.AreEqual(bech32.ToLower(), "zil1n0lvw9dxh4jcljmzkruvexl69t08zs62ds9ats");
        }


        [Test]
        public void FromBech32AddressTest()
        {
            string address = Bech32.FromBech32Address("zil1n0lvw9dxh4jcljmzkruvexl69t08zs62ds9ats");
            Assert.AreEqual(address.ToLower(), "9bfec715a6bd658fcb62b0f8cc9bfa2ade71434a");
            Assert.AreEqual(Bech32.FromBech32Address("zil1fwh4ltdguhde9s7nysnp33d5wye6uqpugufkz7").ToUpper(), "4BAF5FADA8E5DB92C3D3242618C5B47133AE003C");
            Assert.AreEqual(Bech32.FromBech32Address("zil1gjpxry26srx7n008c7nez6zjqrf6p06wur4x3m").ToUpper(), "448261915A80CDE9BDE7C7A791685200D3A0BF4E");
            Assert.AreEqual(Bech32.FromBech32Address("zil1mmgzlktelsh9tspy80f02t0sytzq4ks79zdnkk").ToUpper(), "DED02FD979FC2E55C0243BD2F52DF022C40ADA1E");
            Assert.AreEqual(Bech32.FromBech32Address("zil1z0cxucpf004x50zq9ahkf3qk56e3ukrwaty4g8").ToUpper(), "13F06E60297BEA6A3C402F6F64C416A6B31E586E");
            Assert.AreEqual(Bech32.FromBech32Address("zil1r2gvy5c8c0x8r9v2s0azzw3rvtv9nnenynd33g").ToUpper(), "1A90C25307C3CC71958A83FA213A2362D859CF33");
            Assert.AreEqual(Bech32.FromBech32Address("zil1vfdt467c0khf4vfg7we6axtg3qfan3wlf9yc6y").ToUpper(), "625ABAEBD87DAE9AB128F3B3AE99688813D9C5DF");
            Assert.AreEqual(Bech32.FromBech32Address("zil1x6argztlscger3yvswwfkx5ttyf0tq703v7fre").ToUpper(), "36BA34097F861191C48C839C9B1A8B5912F583CF");
            Assert.AreEqual(Bech32.FromBech32Address("zil16fzn4emvn2r24e2yljnfnk7ut3tk4me6qx08ed").ToUpper(), "D2453AE76C9A86AAE544FCA699DBDC5C576AEF3A");
            Assert.AreEqual(Bech32.FromBech32Address("zil1wg3qapy50smprrxmckqy2n065wu33nvh35dn0v").ToUpper(), "72220E84947C36118CDBC580454DFAA3B918CD97");
            Assert.AreEqual(Bech32.FromBech32Address("zil12rujxpxgjtv55wzu5m8xe454pn56x6pedpl554").ToUpper(), "50F92304C892D94A385CA6CE6CD6950CE9A36839");

        }
    }

}
