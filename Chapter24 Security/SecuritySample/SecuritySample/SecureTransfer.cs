using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecuritySample
{
    public static class SecureTransfer
    {
        private static CngKey _aliceKey;
        private static CngKey _bobKey;
        private static byte[] _alicePubKeyBlob;
        private static byte[] _bobPubKeyBlob;

        public static async Task DisplaySample ()
        {
            
            await RunAsync();
            Console.ReadLine();
        }

        private static async Task RunAsync()
        {
            try
            {
                //创建公钥和私钥 
                CreateKeys();
                byte[] encrytpedData = await AliceSendsDataAsync("this is a secret message for Bob");
                await BobReceivesDataAsync(encrytpedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //生成密钥算法
        private static void CreateKeys()
        {
            //创建Alice的私钥 
            //使用ECDiffieHellmanP521算法
            _aliceKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP521);
            //创建Bob的私钥 
            _bobKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP521);
            //创建Alice的公钥Blob
            _alicePubKeyBlob = _aliceKey.Export(CngKeyBlobFormat.EccPublicBlob);
            //创建Bob的公钥Blob
            _bobPubKeyBlob = _bobKey.Export(CngKeyBlobFormat.EccPublicBlob);
        }

        
        private static async Task<byte[]> AliceSendsDataAsync(string message)
        {
            //传入要发送的信息
            Console.WriteLine($"Alice sends message: {message}");
            //将要发送的信息转化成utf8字节数组
            byte[] rawData = Encoding.UTF8.GetBytes(message);
            byte[] encryptedData = null;
            //创建ECDiffieHellmanCng对象，传入Alice的key初始化它
            using (var aliceAlgorithm = new ECDiffieHellmanCng(_aliceKey))
            //通过 CngKey.Import利用Bob公钥Blob生成公钥
            using (CngKey bobPubKey = CngKey.Import(_bobPubKeyBlob,
                  CngKeyBlobFormat.EccPublicBlob))
            {
                // 使用aliceAlgorithm.DeriveKeyMaterial从而使Alice的密钥对和Bob的公钥创建一个对称密钥
                byte[] symmKey = aliceAlgorithm.DeriveKeyMaterial(bobPubKey);
                Console.WriteLine("Alice creates this symmetric key with " +
                      $"Bobs public key information: { Convert.ToBase64String(symmKey)}");

                //使用AES加密对称密钥
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Key = symmKey;
                    //初始化矢量
                    aes.GenerateIV();
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (var ms = new MemoryStream())
                    {
                        // create CryptoStream and encrypt data to send
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {

                            // write initialization vector not encrypted
                            await ms.WriteAsync(aes.IV, 0, aes.IV.Length);
                            await cs.WriteAsync(rawData, 0, rawData.Length);
                        }
                        encryptedData = ms.ToArray();
                    }
                    //在访问内存流中的加密数据之前必须关闭加密流，否则加密数据就会丢失最后位
                    aes.Clear();
                }
            }
            Console.WriteLine($"Alice: message is encrypted: {Convert.ToBase64String(encryptedData)}"); ;
            Console.WriteLine();
            return encryptedData;
        }

        private static async Task BobReceivesDataAsync(byte[] encryptedData)
        {
            Console.WriteLine("Bob receives encrypted data");
            byte[] rawData = null;

            var aes = new AesCryptoServiceProvider();

            //获得未加密的初始化矢量
            int nBytes = aes.BlockSize >> 3;
            byte[] iv = new byte[nBytes];
            for (int i = 0; i < iv.Length; i++)
                iv[i] = encryptedData[i];

            //用Bob的密钥实例化一个ECDiffieHellmanCng对象
            using (var bobAlgorithm = new ECDiffieHellmanCng(_bobKey))
            //生成Alice的公钥
            using (CngKey alicePubKey = CngKey.Import(_alicePubKeyBlob,
                  CngKeyBlobFormat.EccPublicBlob))
            {
                //使用Alice公钥从DeriveKeyMaterial返回对称密钥
                byte[] symmKey = bobAlgorithm.DeriveKeyMaterial(alicePubKey);
                Console.WriteLine("Bob creates this symmetric key with " +
                      $"Alices public key information: {Convert.ToBase64String(symmKey)}");
                aes.Key = symmKey;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(encryptedData, nBytes, encryptedData.Length - nBytes);
                    }

                    rawData = ms.ToArray();

                    Console.WriteLine($"Bob decrypts message to: {Encoding.UTF8.GetString(rawData)}");
                }
                aes.Clear();
            }
        }

    }
}
