using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RelationUsingConventions
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        //BookId指定Book的外键，如果没有BookId则创建shadow属性
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
