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
    }
}
