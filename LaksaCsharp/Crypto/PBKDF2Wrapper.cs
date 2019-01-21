using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaksaCsharp.Crypto
{
    public class PBKDF2Wrapper
    {
        public byte[] GetDerivedKey(byte[] password, byte[] salt, int iterationCount, int keySize)
        {
            Pkcs5S2ParametersGenerator generator = new Pkcs5S2ParametersGenerator(new Sha256Digest());
            generator.Init(password, salt, iterationCount);
            return ((KeyParameter)generator.GenerateDerivedParameters(keySize * 8)).GetKey();
        }
    }
}
