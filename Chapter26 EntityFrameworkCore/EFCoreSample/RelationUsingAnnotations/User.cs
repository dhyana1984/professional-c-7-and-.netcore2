using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RelationUsingAnnotations
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        //如果相同类型之间存在多个关系，则需要使用InverseProperty特性对属性进行注释，指定另一端的相关属性
        //表示和Book实体的Author属性相关联
        [InverseProperty("Author")] 
        public List<Book> WrittenBooks { get; set; }

        //表示和Book实体的Reviewer属性相关联
        [InverseProperty("Reviewer")]
        public List<Book> ReviewedBooks { get; set; }

        //表示和Book实体的ProjectEditor属性相关联
        [InverseProperty("ProjectEditor")]
        public List<Book> EditedBooks { get; set; }
    }
}
