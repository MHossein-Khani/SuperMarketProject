using SuperMarket.Entities;
using SuperMarket.Services.PurchaseInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistance.EF.PurchaseInvoices
{
    public class EFPurchaseInvoiceRepository : PurchaseInvoiceRepository
    {
        private readonly EFDataContext _dataContext;

        public EFPurchaseInvoiceRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(PurchaseInvoice purchaseInvoice)
        {
            _dataContext.PurchaseInvoices.Add(purchaseInvoice);
        }
    }
}
