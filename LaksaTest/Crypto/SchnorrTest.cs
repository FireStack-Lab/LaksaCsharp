﻿using LaksaCsharp.Crypto;
using LaksaCsharp.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaTest.Crypto
{
    [TestClass]
    public class SchnorrTest
    {
        [TestMethod]
        public void Test()
        {
            Signature signature = null;
            string priv = "B3829F48247210A6D02CD17302DA4B127D51A3AC8484D34064CBF96F83D165C8";
            string pub = "021B780A9CEE44A5F88ACF479C7646F137E87E1106E6542436509BEC21440632BC";
            string msg = "B5935AD79CAF57944113FB42BA7D1D2BB144F2FD09476733BF5258AE0ECD65B3A6E4EFD9C6456AF9268387BB7C0FB5EBEF51A0CC97280D62E62E6FAB9F13216519F6D85AC2C11BB621324C0F2B5DF0A27B37A8999E488B1CF983E481BA4874E60508989883EF6DAD7BF648228D9ADFA8F43EBA0103072F61FAB0C982D68CFDC7A069845E999D69BBDB160BAB0C15962893D13F26259154CD3CB1A2459AA52CC80AA37B34410E697E6EC482365A96602AB5D9B66902C2F467301369AF3E7EEA0DA3729FDAE701CFDC1F4A7DD9AE80D2B03BD3D35750586411DA0518E95A144DA7E72753D7D9E55E3FCF32BDAD55A51C81FD7F6F66B1BEACD0FFD6E20CF876F6A997BAC33C6130D37A592D502DF6272867B48C6F0EB07750DCBE245F64810E70E69C3009EEE4B5C11F1731AA4453AF92463FDDE0C68B9C7FEF23ED47CA66585773C667B6D21C8D2A57E83F4AB2C9A51539DDAA40D89C0726EC6737474C7DBCA19C048FCCF490FF0257D1463493E27A5B5DAE5A32627C57B07DD863F308EDD2B5B5B6524B57FA2F44F1B5296EA27A473EE774490103EE3A21782A434A43225EFD51ACC252684D2A0FD351693B2097175DD87BA3F1CB3F44B779F7C6CA4430064532FDEF0684B6D9544A1483908F912DC9D27557E3CD5121726FFA661DCE8299F569489DD331E64076844D6FEF0247374CB5D4288323258CA90A5F9C255AF39DB6C0";
            string k = "3855CB11A92EE19086E8BC39B773AD90B2E4E7C3D7C443509F762E2F83E3420C";
            string r = "A9C2773C5AEB32348689C982BF30C7F8A216FFDA909204EF51DDD5821E49A306";
            string s = "C11A779F56CE2495004833899629297E6F67FD1ACBCEBE21CB13E9C915DBC73E";
            ECKeyPair key = new ECKeyPair(new BigInteger(priv, 16), new BigInteger(pub, 16));

            while (signature == null)
            {
                signature = Schnorr.TrySign(key, ByteUtil.HexStringToByteArray(msg), new BigInteger(k, 16));
            }
            X9ECParameters secp256k1 = ECNamedCurveTable.GetByName("secp256k1");
            ECPoint pubKeyPoint = secp256k1.Curve.DecodePoint(key.PublicKey.ToByteArray());
            bool res = Schnorr.Verify(ByteUtil.HexStringToByteArray(msg), signature, pubKeyPoint);

            Signature sig = new Signature();
            sig.R = new BigInteger(r, 16);
            sig.S = new BigInteger(s, 16);

            Assert.IsTrue(signature.R.Equals(sig.R));
            Assert.IsTrue(signature.S.Equals(sig.S));
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestAll()
        {

            List<Dictionary<string, string>> jsonData = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            foreach (Dictionary<string, string> item in jsonData)
            {
                Signature signature = null;
                string priv = item["priv"];
                string pub = item["pub"];
                string msg = item["msg"];
                string k = item["k"];
                string r = item["r"];
                string s = item["s"];
                ECKeyPair key = new ECKeyPair(new BigInteger(priv, 16), new BigInteger(pub, 16));

                while (signature == null)
                {
                    signature = Schnorr.TrySign(key, ByteUtil.HexStringToByteArray(msg), new BigInteger(k, 16));
                }
                X9ECParameters secp256k1 = ECNamedCurveTable.GetByName("secp256k1");
                ECPoint pubKeyPoint = secp256k1.Curve.DecodePoint(key.PublicKey.ToByteArray());
                bool res = Schnorr.Verify(ByteUtil.HexStringToByteArray(msg), signature, pubKeyPoint);

                Signature sig = new Signature();
                sig.R = new BigInteger(r, 16);
                sig.S = new BigInteger(s, 16);

                Assert.IsTrue(signature.R.Equals(sig.R));
                Assert.IsTrue(signature.S.Equals(sig.S));
                Assert.IsTrue(res);
            }

        }
    }
}