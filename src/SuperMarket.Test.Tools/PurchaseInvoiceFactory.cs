using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Test.Tools
{
    public static class PurchaseInvoiceFactory
    {
        public static PurchaseInvoice CreateSalesInvoice(string code, string name, int productId)
        {
            return new PurchaseInvoice
            {
                CodeOfProduct = code,
                NameOfProduct = name,
                Number = 2,
                Price = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = productId,
            };
        }
    }
}
