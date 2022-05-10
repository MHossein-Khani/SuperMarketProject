using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceRepository
    {
        void Add(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice FindById(int id);
        void Delete(PurchaseInvoice purchaseInvoice);

    }
}
