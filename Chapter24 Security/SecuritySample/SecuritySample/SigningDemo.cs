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

        //私钥加密签名
        private static CngKey _aliceKeySignature;
        //公钥解密签名
        private static byte[] _alicePubKeyBlob;

        private static void Run()
        {
            //为Alice创建新的密钥对
            InitAliceKeys();
            //将要签名的消息使用 Encoding.UTF8.GetBytes 转换为一个字节数组
            byte[] aliceData = Encoding.UTF8.GetBytes("Alice");
            //给消息签名
            byte[] aliceSignature = CreateSignature(aliceData, _aliceKeySignature);
            //将签名后的信息输出
            Console.WriteLine($"Alice created signature: ${Convert.ToBase64String(aliceSignature)}");
            //使用公钥验证签名
            if(VerifySignature(aliceData, aliceSignature, _alicePubKeyBlob))
            {
                Console.WriteLine("Alice signature verified successfully");
            }
        }

        //创建新的密钥对
        private static void InitAliceKeys()
        {
            // Encoding.UTF8.GetBytes把CngAlgorithm.ECDsaP521作为参数，为算法定义密钥对
            _aliceKeySignature = CngKey.Create(CngAlgorithm.ECDsaP521);
            // 通过_aliceKeySignature.Export导出密钥中的公钥，这个公钥可以提供给其他人来验证
            _alicePubKeyBlob = _aliceKeySignature.Export(CngKeyBlobFormat.GenericPublicBlob);
        }

        //创建签名
        private static byte[] CreateSignature(byte[] data, CngKey key)
        {
            byte[] signature;
            //key是CngKey类型，包含了公钥和私钥
            //将key传入ECDsaCng得到加密算法
            using (var signingAlg = new ECDsaCng(key))
            {
                //使用 signingAlg.SignData 对 要签名的内容以及私钥进行签名
                signature = signingAlg.SignData(data, HashAlgorithmName.SHA512);
                signingAlg.Clear();
            }
            return signature;
        }

        //验证签名
        private static bool VerifySignature(byte[] data, byte[] signature, byte[] pubKey)
        {
            bool retValue = false;
            //使用 CngKey.Import 导入CngKey对象，的到公钥
            using (CngKey key = CngKey.Import(pubKey, CngKeyBlobFormat.GenericPublicBlob))
            //将公钥传入ECDsaCng得到解密签名算法
            using (var signinAlg = new ECDsaCng(key))
            {
                //然后使用signinAlg.VerifyData来验证签名
                retValue = signinAlg.VerifyData(data, signature, HashAlgorithmName.SHA512);
                signinAlg.Clear();
            }
            return retValue;
        }

    }
}
