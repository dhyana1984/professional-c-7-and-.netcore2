using System;
using System.Security.Cryptography;
using System.Text;

namespace SecuritySample
{
    public static class SigningDemo
    {
        public static void DisplaySample()
        {
            Run();
        }

        private static CngKey _aliceKeySignature;
        private static byte[] _alicePubKeyBlob;

        private static void Run()
        {
            InitAliceKeys();
            byte[] aliceData = Encoding.UTF8.GetBytes("Alice");
            byte[] aliceSignature = CreateSignature(aliceData, _aliceKeySignature);

            Console.WriteLine($"Alice created signature: ${Convert.ToBase64String(aliceSignature)}");
            if(VerifySignature(aliceData, aliceSignature, _alicePubKeyBlob))
            {
                Console.WriteLine("Alice signature verified successfully");
            }
        }

        private static void InitAliceKeys()
        {
            _aliceKeySignature = CngKey.Create(CngAlgorithm.ECDsaP521);
            _alicePubKeyBlob = _aliceKeySignature.Export(CngKeyBlobFormat.GenericPublicBlob);
        }

        private static byte[] CreateSignature(byte[] data, CngKey key)
        {
            byte[] signature;
            using (var signingAlg = new ECDsaCng(key))
            {
                signature = signingAlg.SignData(data, HashAlgorithmName.SHA512);
                signingAlg.Clear();
            }
            return signature;
        }

        private static bool VerifySignature(byte[] data, byte[] signature, byte[] pubKey)
        {
            bool retValue = false;
            using (CngKey key = CngKey.Import(pubKey, CngKeyBlobFormat.GenericPublicBlob))
            using (var signinAlg = new ECDsaCng(key))
            {
                retValue = signinAlg.VerifyData(data, signature, HashAlgorithmName.SHA512);
                signinAlg.Clear();
            }
            return retValue;
        }

    }
}
