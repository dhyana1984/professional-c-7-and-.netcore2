using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuSample
{
    //Table attribute有Schema特性
    [Table("MenuCards",Schema ="mc")]
    public class MenuCard
    {
        public int MenuCardId { get; set; }

        [MaxLength(120)]//指定字段长度
        public string Title { get; set; }

        public List<Menu> Menus { get; } = new List<Menu>();

        public override string ToString() => Title;
    }
}
