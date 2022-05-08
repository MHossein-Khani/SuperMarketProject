using SuperMarket.Entities;
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

        public void Add(AddSalesInvoiceDto dto)
        {
            var salesInvoice = new SalesInvoice
            {
                CodeOfProduct = dto.CodeOfProduct,
                NameOfProduct = dto.NameOfProduct,
                Number = dto.Number,
                Date = dto.Date,
                ProductId = dto.ProductId,
            };

            _repository.Add(salesInvoice);
            _unitOfWork.Commit();
        }
    }
}
