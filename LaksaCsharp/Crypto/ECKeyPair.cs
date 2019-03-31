using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Crypto
{
    public class ECKeyPair
    {
        static X9ECParameters CURVE_PARAMS = CustomNamedCurves.GetByName("secp256k1");
        static ECDomainParameters CURVE = new ECDomainParameters(
                CURVE_PARAMS.Curve, CURVE_PARAMS.G, CURVE_PARAMS.N, CURVE_PARAMS.H);

        private BigInteger privateKey;
        private BigInteger publicKey;

        public ECKeyPair(BigInteger privateKey, BigInteger publicKey)
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
        }

        public BigInteger PrivateKey
        {
            get
            {
                return privateKey;
            }
        }

        public BigInteger PublicKey
        {
            get
            {
                return publicKey;
            }
        }

        /* public ECDSASignature sign(byte[] transactionHash)
         {
             ECDsaSigner signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));

             ECPrivateKeyParameters privKey = new ECPrivateKeyParameters(privateKey, Sign.CURVE);
             signer.init(true, privKey);
             BigInteger[] components = signer.generateSignature(transactionHash);

             return new ECDSASignature(components[0], components[1]).toCanonicalised();
         }*/

        /*public static ECKeyPair create(KeyPair keyPair)
        {
            BCECPrivateKey privateKey = (BCECPrivateKey)keyPair.getPrivate();
            BCECPublicKey publicKey = (BCECPublicKey)keyPair.getPublic();

            BigInteger privateKeyValue = privateKey.getD();

            // Ethereum does not use encoded public keys like bitcoin - see
            // https://en.bitcoin.it/wiki/Elliptic_Curve_Digital_Signature_Algorithm for details
            // Additionally, as the first bit is a constant prefix (0x04) we ignore this value
            byte[] publicKeyBytes = publicKey.getQ().getEncoded(false);
            BigInteger publicKeyValue =
                    new BigInteger(1, Arrays.copyOfRange(publicKeyBytes, 1, publicKeyBytes.length));

            return new ECKeyPair(privateKeyValue, publicKeyValue);
        }*/

        public static ECKeyPair Create(BigInteger privateKey)
        {
            return new ECKeyPair(privateKey, PublicKeyFromPrivate(privateKey));
        }

        public static ECKeyPair Create(byte[] privateKey)
        {
            return Create(new BigInteger(privateKey));
        }

        public static BigInteger PublicKeyFromPrivate(BigInteger privKey)
        {
            ECPoint point = publicPointFromPrivate(privKey);

            byte[] encoded = point.GetEncoded(false);
            return new BigInteger(1, encoded.Skip(1).ToArray());  // remove prefix
        }

        public static ECPoint publicPointFromPrivate(BigInteger privKey)
        {
            /*
             * TODO: FixedPointCombMultiplier currently doesn't support scalars longer than the group
             * order, but that could change in future versions.
             */
            if (privKey.BitLength > CURVE.N.BitLength)
            {
                privKey = privKey.Mod(CURVE.N);
            }
            return new FixedPointCombMultiplier().Multiply(CURVE.G, privKey);
        }

    }
}
