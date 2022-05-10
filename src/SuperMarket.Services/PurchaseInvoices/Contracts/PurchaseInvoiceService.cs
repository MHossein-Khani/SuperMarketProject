using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceService : Service
    {
        void Add(AddPurchaseInvoiceDto dto);
        void Update(UpdatePurchaseInvoiceDto dto, int id);
        void Delete(int id);
        List<GetPurchaseInvoiceDto> GetAll();
    }
}
