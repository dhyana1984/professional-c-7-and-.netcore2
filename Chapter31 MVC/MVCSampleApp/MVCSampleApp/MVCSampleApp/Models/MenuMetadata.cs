using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MVCSampleApp.Models
{
    //使用ModelMetadataType(typeof(MenuMetadata))将MenuMetadata连接到Menu
    //假设Menu不能被修改，此处就用partial class来修饰Meunu，即可达到目的
    [ModelMetadataType(typeof(MenuMetadata))]
    public partial class Menu
    {

    }
    public class MenuMetadata
    {
        public int Id { get; set; }
        [Required, StringLength(25)]
        public string Text { get; set; }
        [Display(Name = "Price"), DisplayFormat(DataFormatString = "{0:C}")]
        public double Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [StringLength(10)]
        public string Category { get; set; }
    }
}
