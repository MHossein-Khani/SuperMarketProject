using SuperMarket.Entities;
using SuperMarket.Services.SalesInvoices.Contracts;
using System.Collections.Generic;
using System.Linq;

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

        public void Delete(SalesInvoice salesInvoice)
        {
            _dataContext.SalesInvoices.Remove(salesInvoice);
        }

        public List<GetSalesInvoiceDto> GetByCategory(int categoryId)
        {
           return _dataContext.SalesInvoices.
                Where(p => p.Product.Category.Id == categoryId).
                Select(p => new GetSalesInvoiceDto
                {
                    Id = p.Id,
                    CodeOfProduct = p.CodeOfProduct,
                    NameOfProduct = p.NameOfProduct,
                    Number = p.Number,
                    TotalCost = p.TotalCost,
                    Date = p.Date,
                }).ToList();
        }

        public List<GetSalesInvoiceDto> GetByProduct(int productId)
        {

            return _dataContext.SalesInvoices.
                 Where(p => p.ProductId == productId).
                 Select(p => new GetSalesInvoiceDto
                 {
                     Id = p.Id,
                     CodeOfProduct = p.CodeOfProduct,
                     NameOfProduct = p.NameOfProduct,
                     Number = p.Number,
                     TotalCost = p.TotalCost,
                     Date = p.Date,
                 }).ToList();
        }

        public List<GetSalesInvoiceDto> GetAll()
        {
            return _dataContext.SalesInvoices.
                Select(p => new GetSalesInvoiceDto
                {
                    Id = p.Id,
                    CodeOfProduct = p.CodeOfProduct,
                    NameOfProduct = p.NameOfProduct,
                    Number = p.Number,
                    TotalCost = p.TotalCost,
                    Date = p.Date,
                }).ToList();
        }
    }
}
