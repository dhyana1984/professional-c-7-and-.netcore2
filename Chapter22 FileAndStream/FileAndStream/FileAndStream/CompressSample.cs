using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace FileAndStream
{
    public static class CompressSample
    {
        //压缩
        //生成的文件是*.gzip
        public static void CompressFile(string fileName, string compressedFileName)
        {
            using (FileStream inputStream = File.OpenRead(fileName))
            {
                FileStream outputStream = File.OpenWrite(compressedFileName);
                //DeflateStream构造函数压缩文件
                //CompressionMode.Compress是压缩对象
                using (var compressStream = new DeflateStream(outputStream, CompressionMode.Compress))
                {
                    //将要压缩的文件流copy到DeflateStream 对象
                    inputStream.CopyTo(compressStream);
                }
            }
        }

        //解压缩
        public static void DecompressFile(string fileName)
        {
            FileStream inputStream = File.OpenRead(fileName);
            using (MemoryStream outputStream = new MemoryStream())
            //CompressionMode.Decompress表示解压缩
            using (var compressStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            {
                compressStream.CopyTo(outputStream);
                outputStream.Seek(0, SeekOrigin.Begin);
                //leaveOpen是配置StreamReader在关闭读取器以后，outputStream也可以使用读取器
                using (var reader = new StreamReader(outputStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 4096, leaveOpen: true))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
                //在这里仍然可以可以使用outputStream，即使StreamReader已经关闭了
            }
        }

        //使用brotli压缩文件
        //生成的文件是*.brotli
        public static void CompressFileWithBrotli(string fileName, string compressedFileName)
        {
            using (FileStream inputStream = File.OpenRead(fileName))
            {
                FileStream outputStream = File.OpenWrite(compressedFileName);
                //使用BrotliStream代替DeflateStream
                using (var compressStream = new BrotliStream(outputStream, CompressionMode.Compress))
                {
                    inputStream.CopyTo(compressStream);
                }
            }
        }

        public static void DecompressFileWithBrotli(string fileName)
        {
            FileStream inputStream = File.OpenRead(fileName);
            using (MemoryStream outputStream = new MemoryStream())
            using (var compressStream = new BrotliStream(inputStream, CompressionMode.Decompress))
            {
                compressStream.CopyTo(outputStream);
                outputStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outputStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 4096, leaveOpen: true))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }

        //压缩文件
        public static void CreateZipFile(string directory, string zipFile)
        {
            InitSampleFilesForZip(directory);
            string destDirectory = Path.GetDirectoryName(zipFile);
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }
            FileStream zipStream = File.Create(zipFile);
            //创建ZipArchive对象，包含多个ZipArchiveEntry对象
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    //为目录下每个文件创建ZipArchiveEntry对象
                    ZipArchiveEntry entry = archive.CreateEntry(Path.GetFileName(file));
                    using (FileStream inputStream = File.OpenRead(file))
                    using (Stream outputStream = entry.Open())
                    {
                        //将文件写入ZipArchiveEntry的Open方法创建的Stream
                        inputStream.CopyTo(outputStream);
                    }
                }
            }
        }

        private static void InitSampleFilesForZip(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);

                for (int i = 0; i < 10; i++)
                {
                    string destFileName = Path.Combine(directory, $"test{i}.txt");

                    File.Copy("Test.txt", destFileName);
                }

            } // else nothing to do, using existing files from the directory
        }
    }
}
