using SuperMarket.Services.SalesInvoices.Contracts;
using System.Collections.Generic;

namespace SuperMarket.Services.SalesInvoices
{
    public interface SalesInvoiceService
    {
        void Add(AddSalesInvoiceDto dto);
        void Update(UpdateSalesInvoiceDto dto, int id);
        void Delete(int id);
        List<GetSalesInvoiceDto> GetByCategory(int categoryId);
        List<GetSalesInvoiceDto> GetByProduct(int productId);
        List<GetSalesInvoiceDto> GetAll();
    }
}
