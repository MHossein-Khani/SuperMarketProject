using SuperMarket.Services.SalesInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices
{
    public interface SalesInvoiceService
    {
        void Add(AddSalesInvoiceDto dto);
        void Update(UpdateSalesInvoiceDto dto, int id);
        void Delete(int id);
        List<GetSalesInvoiceDto> GetByCategory(int categoryId);
    }
}
