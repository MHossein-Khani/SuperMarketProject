using System;
using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Services.PurchaseInvoices.Contracts
{
    public class AddPurchaseInvoiceDto
    {
        [Required]
        public string CodeOfProduct { get; set; }

        [Required]
        public string NameOfProduct { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
