using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public class UpdateSalesInvoiceDto
    {
        public string CodeOfProduct { get; set; }
        public string NameOfProduct { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int TotalCost { get; set; }

        public int ProductId { get; set; }
    }
}
