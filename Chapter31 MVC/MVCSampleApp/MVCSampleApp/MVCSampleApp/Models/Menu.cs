using System;
using System.ComponentModel.DataAnnotations;

namespace MVCSampleApp.Models
{
    public partial class Menu
    {
        public int Id { get; set; }

        [Display(Name ="Menu")]
        [Required, StringLength(50)]
        public string Text { get; set; }


        [Display(Name = "Price"), DisplayFormat(DataFormatString = "{0:c}")]
        public double Price { get; set; }

        [DataType(DataType.Date)] //这个注解表示显示为日期格式
        public DateTime Date { get; set; }

        [StringLength(10)]
        public string Category { get; set; }
    }
}
