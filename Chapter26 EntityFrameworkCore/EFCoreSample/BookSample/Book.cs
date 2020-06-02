using System;
namespace BookSample
{
    public class Book
    {
        private Book()
        {
            
        }

        public Book(string title, string publisher)
        {
            Title = title;
            _publisher = publisher;
        }
        //只有字段，没有属性，可以在OnModelCreating中用数据库字段映射
        // modelBuilder.Entity<Book>().Property<int>("_bookId").HasColumnName(BookId).HasField("_bookId").IsRequired();
        private int _bookId = 0;
        public string Title { get; set; }
        private string _publisher;
        public string Publisher => _publisher;

        public override string ToString() => $"id: {_bookId}, title: {Title}, publisher: {Publisher}";
    }
}
