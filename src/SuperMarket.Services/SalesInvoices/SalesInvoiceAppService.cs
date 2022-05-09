using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Services.SalesInvoices.Exceptions;
using System.Collections.Generic;

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
            var productInventory = _repository.
                NumberOfProductInventory(dto.ProductId);
            if (productInventory < dto.Number)
            {
                throw new TheNumberOfProductsIsLessThanTheNumberRequestedException();
            }

            var salesInvoice = new SalesInvoice
            {
                CodeOfProduct = dto.CodeOfProduct,
                NameOfProduct = dto.NameOfProduct,
                Number = dto.Number,
                Date = dto.Date,
                ProductId = dto.ProductId,
            };

            _repository.ReduceInventory(dto.ProductId, dto.Number);
            _repository.Add(salesInvoice);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var salesInvoice = _repository.FindById(id);
            var lastProductId = salesInvoice.ProductId;
            var lastProductInventory = salesInvoice.Number;

            _repository.Delete(salesInvoice);
            _repository.AddInventory(lastProductId, lastProductInventory);
            _unitOfWork.Commit();
        }

        public List<GetSalesInvoiceDto> GetByCategory(int categoryId)
        {
            return _repository.GetByCategory(categoryId);
        }

        public void Update(UpdateSalesInvoiceDto dto, int id)
        {
            int productInventory = _repository.
              NumberOfProductInventory(dto.ProductId);
            if (productInventory < dto.Number)
            {
                throw new InventoryIsOutOfStockException();
            }

            var salesInvoice = _repository.FindById(id);

            var lastProductId = salesInvoice.ProductId;
            var lastProductInventory = salesInvoice.Number;

            salesInvoice.CodeOfProduct = dto.CodeOfProduct;
            salesInvoice.NameOfProduct = dto.NameOfProduct;
            salesInvoice.Number = dto.Number;
            salesInvoice.TotalCost = dto.TotalCost;
            salesInvoice.Date = dto.Date;
            salesInvoice.ProductId = dto.ProductId;

            _repository.AddInventory(lastProductId, lastProductInventory);
            _repository.ReduceInventory(dto.ProductId, dto.Number);
            _unitOfWork.Commit();
        }
    }
}
