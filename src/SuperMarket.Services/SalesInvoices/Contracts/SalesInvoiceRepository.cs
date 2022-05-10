using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public interface SalesInvoiceRepository : Repository
    {
        void Add(SalesInvoice salesInvoice);       
        SalesInvoice FindById(int id);
        void Delete(SalesInvoice salesInvoice);
        List<GetSalesInvoiceDto> GetByCategory(int categoryId);
        List<GetSalesInvoiceDto> GetByProduct(int productId);
        List<GetSalesInvoiceDto> GetAll();
        bool IsProductUsedInSalesInvoice(int productId);
    }
}
