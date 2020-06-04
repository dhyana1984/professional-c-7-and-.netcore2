using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RelationUsingConventions
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        //这里表示一对多关系
        public List<Chapter> Chapters { get; } = new List<Chapter>();
        //DB不会为User创建一个外键，而是创建一个AuthuorUserId的shadow属性 
        public User Author { get; set; }
  
    }
}
