using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RelationUsingConventions
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        //一对多关系
        public List<Book> AuthoredBooks { get; set; }

    }
}
