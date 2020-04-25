using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Document;

namespace Collections
{
    public class QueueSample
    {
        public QueueSample()
        {
        }
        
        public async Task DisplaySample()
        {
            var dm = new DocumentManager();
            //ProcessDocuments在一个单独的任务中处理队列的文档，能从外部访问的唯一方法是Start()
            Task processDocuments = ProcessDocuments.Start(dm);

            for (int i = 0; i < 10; i++)
            {
                
                var doc = new Document($"Doc {i.ToString()}", "content");
                dm.AddDocument(doc);
                Console.WriteLine($"Added document {doc.Title}");
                await Task.Delay(new Random().Next(20));
            }
            await processDocuments;
        }
    }

    public class DocumentManager
    {
        private readonly object _syncQueue = new object();
        private readonly Queue<Document> _documentQueue = new Queue<Document>();

        public void AddDocument(Document doc)
        {
            lock (_syncQueue)
            {
                _documentQueue.Enqueue(doc);
            }
        }

        public Document GetDocument()
        {
            Document doc = null;
            lock (_syncQueue)
            {
                doc = _documentQueue.Dequeue();
            }
            return doc;
        }

        public bool IsDocumentAvailable => _documentQueue.Count > 0;
    }

    public class ProcessDocuments
    {
        private DocumentManager _documentManager;
        public static Task Start(DocumentManager dm) =>
            Task.Run(new ProcessDocuments(dm).Run); //在Start()中实例化了一个新任务

        protected ProcessDocuments(DocumentManager dm) =>
            _documentManager = dm?? throw new ArgumentNullException(nameof(dm));

        //定义Run方法微任务的启动方法
        protected async Task Run()
        {
            //定义成一个无限循环函数，可以一直处理
            while (true)
            {
                if (_documentManager.IsDocumentAvailable)
                {
                    Document doc = _documentManager.GetDocument();
                    Console.WriteLine($"Process document {doc.Title}");
                }
                await Task.Delay(new Random().Next(20));
            }
        }
    }
}
