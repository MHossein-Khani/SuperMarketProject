using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public class GetSalesInvoiceDto
    {
        public int Id { get; set; }
        public string CodeOfProduct { get; set; }
        public string NameOfProduct { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int TotalCost { get; set; }

    }
}
