using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseInvoices.Contracts
{
    public class GetPurchaseInvoiceDto
    {
        public int Id { get; set; }
        public string CodeOfProduct { get; set; }
        public string NameOfProduct { get; set; }
        public int Number { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }

        public int ProductId { get; set; }
    }
}
