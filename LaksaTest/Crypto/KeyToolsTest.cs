using LaksaCsharp.Crypto;
using NUnit.Framework;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaksaTest.Crypto
{
    [TestFixture]
    public class KeyToolsTest
    {
        static private X9ECParameters secp256k1 = ECNamedCurveTable.GetByName("secp256k1");

        [Test]
        public void GenerateKeyPair()
        {
            ECKeyPair keys = KeyTools.GenerateKeyPair();
            ECPoint pubKey = secp256k1.Curve.DecodePoint(keys.PublicKey.ToByteArray());
            Assert.AreEqual(keys.PrivateKey.CompareTo(BigInteger.Zero), 1);
            Assert.IsTrue(pubKey.IsValid());
        }

        [Test]
        public void GeneratePrivateKey()
        {
            int i = 0;
            while (i < 100000)
            {
                i++;
                string privateKey = KeyTools.GeneratePrivateKey();
                if (privateKey.Length != 64)
                {
                    throw new Exception("generator err");
                }
                Console.WriteLine(privateKey);
            }
        }

        [Test]
        public void TestKeyAndAddress()
        {
            int i = 0;
            while (i < 100)
            {
                i++;
                string privateKey = KeyTools.GeneratePrivateKey();
                if (privateKey.Length != 64)
                {
                    throw new Exception("generator err");
                }
                string publicKey = KeyTools.GetPublicKeyFromPrivateKey(privateKey, true);

                Assert.AreEqual(KeyTools.GetAddressFromPrivateKey(privateKey), KeyTools.GetAddressFromPublicKey(publicKey.ToLower()));
            }
        }

        [Test]
        public void GenerateRandomBytes()
        {
            byte[] bytes = KeyTools.GenerateRandomBytes(32);
            Assert.NotNull(bytes);
            Assert.IsTrue(32 == bytes.Length);
        }

        [Test]
        public void GetPublicKeyFromPrivateKey()
        {
            String privateKey = "24180e6b0c3021aedb8f5a86f75276ee6fc7ff46e67e98e716728326102e91c9";
            String publicKey = KeyTools.GetPublicKeyFromPrivateKey(privateKey, false);
            Assert.AreEqual(publicKey.ToLower(), "04163fa604c65aebeb7048c5548875c11418d6d106a20a0289d67b59807abdd299d4cf0efcf07e96e576732dae122b9a8ac142214a6bc133b77aa5b79ba46b3e20");
            privateKey = "b776d8f068d11b3c3f5b94db0fb30efea05b73ddb9af1bbd5da8182d94245f0b";
            publicKey = KeyTools.GetPublicKeyFromPrivateKey(privateKey, false);
            Assert.AreEqual(publicKey.ToLower(), "04cfa555bb63231d167f643f1a23ba66e6ca1458d416ddb9941e95b5fd28df0ac513075403c996efbbc15d187868857e31cf7be4d109b4f8cb3fd40499839f150a");
            privateKey = "42476c6c866edebe07b536dc8114b1c33babd53a";
            System.Console.WriteLine(KeyTools.GetPublicKeyFromPrivateKey(privateKey, true));
        }

        [Test]
        public void GetAddressFromPrivateKey()
        {
            String privateKey = "a2315cc0cd0a4a0b5b58e2e270017d77ac79302d53199d0cf351669497466840";
            String address = KeyTools.GetAddressFromPrivateKey(privateKey);
            System.Console.WriteLine(address.ToLower());
        }

        [Test]
        public void GetAddressFromPublicKey()
        {
            System.Console.WriteLine(KeyTools.GetAddressFromPublicKey("03B1785FB998CF2A650D064A7E4F6C34EAB5FC44600C81D4240644C89430618AF1"));
            String noPad = "38959e0b7b9c545dc055ab668f8fbaef207e845c590eca4b14993619fff0f723d";
            String padded = "038959e0b7b9c545dc055ab668f8fbaef207e845c590eca4b14993619fff0f723d";

            Assert.AreEqual(KeyTools.GetAddressFromPublicKey(noPad), KeyTools.GetAddressFromPublicKey(padded));
        }
    }


}
