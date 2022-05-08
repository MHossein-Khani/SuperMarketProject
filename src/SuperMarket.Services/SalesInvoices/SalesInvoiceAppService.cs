using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.SalesInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices
{
    public class SalesInvoiceAppService : SalesInvoiceService
    {
        private readonly SalesInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public SalesInvoiceAppService(SalesInvoiceRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
