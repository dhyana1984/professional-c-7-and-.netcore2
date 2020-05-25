using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecuritySample
{
    public static class RSASignature
    {
        private static CngKey _aliceKey;
        private static byte[] _alicePubKeyBlob;

        public  static void DisplaySample()
        {
            Run();
        }

        static void Run()
        {
            AliceTasks(out byte[] document, out byte[] hash, out byte[] signature);
            BobTasks(document, hash, signature);
        }

        static void AliceTasks(out byte[] data, out byte[] hash, out byte[] signature)
        {
            //创建Alice需要的密钥
            InitAliceKeys();
            //将消息转成字节数组
            data = Encoding.UTF8.GetBytes("Best greetings from Alice");
            //散列字节数组
            hash = HashDocument(data);
            //添加一个签名
            signature = AddSignatureToHash(hash, _aliceKey);
        }

        private static void InitAliceKeys()
        {
            //创建Alice所需密钥，使用RSA算法
            _aliceKey = CngKey.Create(CngAlgorithm.Rsa);
            //创建公钥
            _alicePubKeyBlob = _aliceKey.Export(CngKeyBlobFormat.GenericPublicBlob);
        }

        //创建散列
        private static byte[] HashDocument(byte[] data)
        {
            //使用SHA384对原文创建散列
            using (var hashAlg = SHA384.Create())
            {
                return hashAlg.ComputeHash(data);
            }
        }

        //添加签名，保证文档来自于Alice
        private static byte[] AddSignatureToHash(byte[] hash, CngKey key)
        {
            //使用RSACng类给散列签名
            using (var signingAlg = new RSACng(key))
            {
                //SignHash方法创建签名
                //HashAlgorithmName.SHA384是创建散列的算法，SignHash必须知道
                byte[] signed = signingAlg.SignHash(hash, HashAlgorithmName.SHA384, RSASignaturePadding.Pss);
                return signed;
            }
        }

        static void BobTasks(byte[] data, byte[] hash, byte[] signature)
        {
            //获得Alice公钥
            CngKey aliceKey = CngKey.Import(_alicePubKeyBlob, CngKeyBlobFormat.GenericPublicBlob);
            //验证签名是否有效
            if (!IsSignatureValid(hash, signature, aliceKey))
            {
                Console.WriteLine("signature not valid");
                return;
            }
            //验证文档是否没有变化
            if (!IsDocumentUnchanged(hash, data))
            {
                Console.WriteLine("document was changed");
                return;
            }
            Console.WriteLine("signature valid, document unchanged");
            Console.WriteLine($"document from Alice: {Encoding.UTF8.GetString(data)}");
        }

        private static bool IsSignatureValid(byte[] hash, byte[] signature, CngKey key)
        {
            //使用Alice的公钥创建RSACng实例
            using (var signingAlg = new RSACng(key))
            {
                //VerifyHash方法传递散列，签名，散列算法，来验证签名
                return signingAlg.VerifyHash(hash, signature, HashAlgorithmName.SHA384, RSASignaturePadding.Pss);
            }
        }

        private static bool IsDocumentUnchanged(byte[] hash, byte[] data)
        {
            //再次散列原文
            byte[] newHash = HashDocument(data);
            //使用SequenceEqual验证再次散列的原文是否等于散列值
            //SequenceEqual用于比较list的元素引用和顺序是否相同
            return newHash.SequenceEqual(hash);
        }
    }
}
