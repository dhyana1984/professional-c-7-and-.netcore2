using System;
using System.Collections.Generic;

namespace Generic
{
    public interface IDocument
    {
        string Title { get; }
        string Content { get; }
    }

    public class Document : IDocument
    {
        public Document(string title, string content)
        {
            Title = title;
            Content = content;
        }
        public string Title { get; }
        public string Content { get; }
    }

    //约束TDocument必须实现IDocument接口
    public class DocumentManager<TDocument> where TDocument:IDocument
    {
        private readonly Queue<TDocument> _documentQueue = new Queue<TDocument>();
        private readonly object _lockQueue = new object();
        public void AddDocument(TDocument doc)
        {
            lock (_lockQueue)
            {
                _documentQueue.Enqueue(doc);
            }
        }

        public bool IsDocumentAvaliable => _documentQueue.Count > 0;

        public TDocument GetDocument()
        {
            //default用来初始化泛型，如果是引用类型就是null，如果是值类型就是0
            TDocument doc = default;
            lock (_lockQueue)
            {
                doc = _documentQueue.Dequeue();
            }
            return doc;
        }

        public void DisplayAllDocuments()
        {
            foreach (var item in _documentQueue)
            {
                Console.WriteLine(item.Title);
            }
        }
    }
}
