using SuperMarket.Entities;
using SuperMarket.Services.SalesInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistance.EF.SalesInvoices
{
    public class EFSalesInvoiceRepository : SalesInvoiceRepository
    {
        private readonly EFDataContext _dataContext;

        public EFSalesInvoiceRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(SalesInvoice salesInvoice)
        {
            _dataContext.SalesInvoices.Add(salesInvoice);
        }

        public SalesInvoice FindById(int id)
        {
            return _dataContext.SalesInvoices.Find(id);
        }

        public int NumberOfProductInventory(int productId)
        {
            var product = _dataContext.products.FirstOrDefault(p => p.Id == productId);
            
            var inventory = product.Inventory;

            return inventory;
        }
    }
}
