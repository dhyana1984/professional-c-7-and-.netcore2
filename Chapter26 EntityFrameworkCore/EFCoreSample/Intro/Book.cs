using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intro
{
    //映射到表
    [Table("Books")]
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        //映射该字段不能为空
        [Required]
        //映射该字段长度
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(30)]
        public string Publisher { get; set; }

    }
}
