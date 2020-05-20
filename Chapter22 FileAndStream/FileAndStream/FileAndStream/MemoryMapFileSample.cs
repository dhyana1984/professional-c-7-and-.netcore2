using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;

namespace FileAndStream
{
    public static class MemoryMapFileSample
    {
        //基础设施创建任务和发出信号
        //当一个任务创建好内存映射文件时，可以使用_mapCreated.Set()向_mapCreated发射信号，通知其他任务已经创建好内存映射文件
        static private ManualResetEventSlim _mapCreated = new ManualResetEventSlim(initialState: false);
        //写入共享内存完成后调用_dataWrittenEvent.Set() 通知读取器可以开始读取
        static private ManualResetEventSlim _dataWrittenEvent = new ManualResetEventSlim(initialState: false);
        private const string MAPNAME = "SampleMap";

        //写入内存
        private static async Task WriterAsync()
        {
            try
            {
                //创建基于内存的内存映射文件，如果访问物理文件，可以使用CreateFromFile方法
                // MemoryMappedFile mappedFile = MemoryMappedFile.CreateFromFile("./memoryMappedFile", FileMode.Create, MAPNAME, 10000);
                using (MemoryMappedFile mappedFile = MemoryMappedFile.CreateOrOpen(MAPNAME, 10000, MemoryMappedFileAccess.ReadWrite))
                {
                    //创建内存映射文件后，给事件_mapCreated发送信号，给其他任务提供信息，说明已经创建了内存映射文件
                    _mapCreated.Set(); 
                    Console.WriteLine("shared memory segment created");
                    //创建视图访问器用于写入，将原始数据写入共享内存
                    using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(0, 10000, MemoryMappedFileAccess.Write))
                    {
                        for (int i = 0, pos = 0; i < 100; i++, pos += 4)
                        {
                            //根据位置信息，指定写入的位置
                            accessor.Write(pos, i);
                            Console.WriteLine($"written {i} at position {pos}");
                            await Task.Delay(10);
                        }
                        //全部写完以后给事件发出信号，通知读取器可以开始读取了
                        _dataWrittenEvent.Set(); 
                        Console.WriteLine("data written");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"writer {ex.Message}");
            }
        }

        //读取器
        private static void Reader()
        {
            try
            {
                Console.WriteLine("reader");
                _mapCreated.Wait();
                Console.WriteLine("reader starting");

                //简历内存映射文件
                using (MemoryMappedFile mappedFile = MemoryMappedFile.OpenExisting(MAPNAME, MemoryMappedFileRights.Read))
                {
                    //通过视图读取器打开共享内存映射文件
                    using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(0, 10000, MemoryMappedFileAccess.Read))
                    {
                        //等待写入完成
                        _dataWrittenEvent.Wait();
                        Console.WriteLine("reading can start now");
                        //开始读取
                        for (int i = 0; i < 400; i += 4)
                        {
                            int result = accessor.ReadInt32(i);
                            Console.WriteLine($"reading {result} from position {i}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"reader {ex.Message}");
            }
        }

        //使用流创建内存映射文件
        private static async Task WriterUsingStreamsAsync()
        {
            try
            {
                using (MemoryMappedFile mappedFile = MemoryMappedFile.CreateOrOpen(MAPNAME, 10000, MemoryMappedFileAccess.ReadWrite))
                // MemoryMappedFile mappedFile = MemoryMappedFile.CreateFromFile("./memoryMappedFile", FileMode.Create, MAPNAME, 10000);
                {
                    _mapCreated.Set(); // signal shared memory segment created
                    Console.WriteLine("shared memory segment created");
                    //MemoryMappedFile的CreateViewStream返回MemoryMappedViewStream，类似MemoryMappedViewAccessor
                    MemoryMappedViewStream stream = mappedFile.CreateViewStream(0, 10000, MemoryMappedFileAccess.Write);
                    //使用StreamWriter来读取流
                    using (var writer = new StreamWriter(stream))
                    {
                        //每次写入的内容刷新缓存
                        writer.AutoFlush = true;
                        for (int i = 0; i < 100; i++)
                        {
                            string s = $"some data {i}";
                            Console.WriteLine($"writing {s} at {stream.Position}");
                            await writer.WriteLineAsync(s);
                        }
                    }
                    _dataWrittenEvent.Set(); // signal all data written
                    Console.WriteLine("data written");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"writer {ex.Message}");
            }
        }


        //使用流读取内存映射文件
        private static async  Task ReaderUsingStreamsAsync()
        {
            try
            {
                Console.WriteLine("reader");
                _mapCreated.Wait();
                Console.WriteLine("reader starting");

                using (MemoryMappedFile mappedFile = MemoryMappedFile.OpenExisting(MAPNAME, MemoryMappedFileRights.Read))
                {
                    MemoryMappedViewStream stream = mappedFile.CreateViewStream(0, 10000, MemoryMappedFileAccess.Read);
                    using (var reader = new StreamReader(stream))
                    {
                        _dataWrittenEvent.Wait();
                        Console.WriteLine("reading can start now");

                        for (int i = 0; i < 100; i++)
                        {
                            long pos = stream.Position;
                            string s = await reader.ReadLineAsync();
                            Console.WriteLine($"read {s} from {pos}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"reader {ex.Message}");
            }
        }
    }

}

