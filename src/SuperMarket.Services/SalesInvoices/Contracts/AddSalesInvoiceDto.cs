using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public class AddSalesInvoiceDto
    {
        [Required]
        public string CodeOfProduct { get; set; }
        
        [Required]
        public string NameOfProduct { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int TotalCost { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
