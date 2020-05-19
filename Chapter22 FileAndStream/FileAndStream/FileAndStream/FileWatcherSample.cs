using System;
using System.IO;

namespace FileAndStream
{
    public static class FileWatcherSample
    {
        //文件监听器
        private static FileSystemWatcher s_watcher;
        public static void WatchFiles(string path, string filter)
        {
            //FileSystemWatcher构造函数可以传入要监控的目录地址和一个filter，例如*.txt
            s_watcher = new FileSystemWatcher(path, filter)
            {
                //监控子文件夹
                IncludeSubdirectories = true
            };
            //绑定监听的事件处理程序
            s_watcher.Created += OnFileChanged;
            s_watcher.Changed += OnFileChanged;
            s_watcher.Deleted += OnFileChanged;
            //Renamed的处理程序要单独定义，因为使用RenamedEventArgs
            s_watcher.Renamed += OnFileRenamed;

            s_watcher.EnableRaisingEvents = true;
            Console.WriteLine("watching file changes...");
        }

        //重命名的事件类型是RenamedEventArgs
        private static void OnFileRenamed(object sender, RenamedEventArgs e) =>
            Console.WriteLine($"file {e.OldName} {e.ChangeType} to {e.Name}");

        //新增，变更，删除的事件类型是FileSystemEventArgs
        private static void OnFileChanged(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"file {e.Name} {e.ChangeType}");
    }
}
