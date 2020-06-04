using System;
namespace RelationUsingFluentAPI
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        //用来关联外键
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
