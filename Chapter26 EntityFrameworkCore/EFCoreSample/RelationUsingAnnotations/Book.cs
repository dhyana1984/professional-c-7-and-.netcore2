using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RelationUsingAnnotations
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; } = new List<Chapter>();

        public int? AuthorId { get; set; }
        //使用ForeignKey定义外键，并强制定义关系，实现级联删除。即Book删除时Author，Reviewer，ProjectEditor也会被删除
        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        public int? ReviewerId { get; set; }
        [ForeignKey(nameof(ReviewerId))]
        public User Reviewer { get; set; }

        public int? ProjectEditorId { get; set; }
        [ForeignKey(nameof(ProjectEditorId))]
        public User ProjectEditor { get; set; }
    }
}
