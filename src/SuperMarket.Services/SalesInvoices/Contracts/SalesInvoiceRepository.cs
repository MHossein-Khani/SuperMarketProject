using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public interface SalesInvoiceRepository
    {
        void Add(SalesInvoice salesInvoice);
        int NumberOfProductInventory(int productId);
        SalesInvoice FindById(int id);
        void ReduceInventory(int productId, int salesNumber);
        void AddInventory(int productId, int salesNumber);
        void Delete(SalesInvoice salesInvoice);
        List<GetSalesInvoiceDto> GetByCategory(int categoryId);
    }
}
