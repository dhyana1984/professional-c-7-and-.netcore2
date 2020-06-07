using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuSample
{
    [Table("Menus", Schema ="mc")]
    public class Menu
    {
        public int MenuId { get; set; }

        [MaxLength(50)]
        public string Text { get; set; }

        [Column(TypeName ="Money")]
        public decimal Price { get; set; }

        public int MenuCardId { get; set; }

        public MenuCard MenuCard { get; set; }

        public string Allergens { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
