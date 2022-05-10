using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceRepository : Repository
    {
        void Add(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice FindById(int id);
        void Delete(PurchaseInvoice purchaseInvoice);
        List<GetPurchaseInvoiceDto> GetAll();

    }
}
