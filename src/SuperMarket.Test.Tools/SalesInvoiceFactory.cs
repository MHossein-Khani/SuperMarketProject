using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Test.Tools
{
    public static class SalesInvoiceFactory
    {
        public static SalesInvoice CreateSalesInvoice(string code, string name, int productId)
        {
            return new SalesInvoice
            {
                CodeOfProduct = code,
                NameOfProduct = name,
                Number = 2,
                TotalCost = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = productId,
            };
        }
    }
}
