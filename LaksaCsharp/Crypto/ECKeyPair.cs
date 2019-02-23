using Nethereum.Signer.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Crypto
{
    public class ECKeyPair
    {
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
    }
}
