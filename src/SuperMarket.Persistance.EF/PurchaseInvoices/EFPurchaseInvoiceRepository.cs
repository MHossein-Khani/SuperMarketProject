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

        public void Delete(PurchaseInvoice purchaseInvoice)
        {
            _dataContext.PurchaseInvoices.Remove(purchaseInvoice);
        }

        public PurchaseInvoice FindById(int id)
        {
            return _dataContext.PurchaseInvoices.Find(id);
        }

        public List<GetPurchaseInvoiceDto> GetAll()
        {
            return _dataContext.PurchaseInvoices.
                Select(p => new GetPurchaseInvoiceDto
                {
                    Id = p.Id,
                    CodeOfProduct = p.CodeOfProduct,
                    NameOfProduct = p.NameOfProduct,
                    Number = p.Number,
                    Price = p.Price,
                    Date = p.Date,
                }).ToList();
        }
    }
}
