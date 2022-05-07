using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Products.Cantracts
{
    public class AddProductDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Inventory { get; set; }
        [Required]
        public int MinimumInventory { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
