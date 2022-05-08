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


    }
}
