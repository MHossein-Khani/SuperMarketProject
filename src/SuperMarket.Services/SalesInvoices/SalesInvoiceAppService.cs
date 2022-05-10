using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Services.SalesInvoices.Exceptions;
using System.Collections.Generic;

namespace SuperMarket.Services.SalesInvoices
{
    public class SalesInvoiceAppService : SalesInvoiceService
    {
        private readonly SalesInvoiceRepository _repository;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public SalesInvoiceAppService(SalesInvoiceRepository repository,
            ProductRepository productRepository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddSalesInvoiceDto dto)
        {
            var productInventory = _productRepository.
               NumberOfProductInventory(dto.ProductId);
            if (productInventory < dto.Number)
            {
                throw new TheNumberOfProductsIsLessThanTheNumberRequestedException();
            }

            SalesInvoice salesInvoice = CreateSalesInvoice(dto);

            var newProductInSalesInvoice = _productRepository.FindById(dto.ProductId);
            newProductInSalesInvoice.Inventory -= dto.Number;

            _repository.Add(salesInvoice);
            _unitOfWork.Commit();
        }

        private static SalesInvoice CreateSalesInvoice(AddSalesInvoiceDto dto)
        {
            return new SalesInvoice
            {
                CodeOfProduct = dto.CodeOfProduct,
                NameOfProduct = dto.NameOfProduct,
                Number = dto.Number,
                Date = dto.Date,
                ProductId = dto.ProductId,
            };
        }

        public void Delete(int id)
        {
            var salesInvoice = _repository.FindById(id);
            var lastProductId = salesInvoice.ProductId;
            var lastProductInventory = salesInvoice.Number;

            _repository.Delete(salesInvoice);

            var lastProductInSalesInvoice = _productRepository.FindById(lastProductId);
            lastProductInSalesInvoice.Inventory += lastProductInventory;

            _unitOfWork.Commit();
        }

        public List<GetSalesInvoiceDto> GetAll()
        {
            return _repository.GetAll();
        }

        public List<GetSalesInvoiceDto> GetByCategory(int categoryId)
        {
            return _repository.GetByCategory(categoryId);
        }

        public List<GetSalesInvoiceDto> GetByProduct(int productId)
        {
            return _repository.GetByProduct(productId);
        }

        public void Update(UpdateSalesInvoiceDto dto, int id)
        {
            int productInventory = _productRepository.
              NumberOfProductInventory(dto.ProductId);
            if (productInventory < dto.Number)
            {
                throw new InventoryIsOutOfStockException();
            }

            var purchaseInvoice = _repository.FindById(id);

            var lastProductId = purchaseInvoice.ProductId;
            var lastProductInventory = purchaseInvoice.Number;

            UpdateSalesInvoiceByDto(dto, purchaseInvoice);

            SetProductInventory(dto, lastProductId, lastProductInventory);

            _unitOfWork.Commit();
        }

        private void SetProductInventory(UpdateSalesInvoiceDto dto, int lastProductId, int lastProductInventory)
        {
            var lastProductInPurchaseInvoice = _productRepository.FindById(lastProductId);
            lastProductInPurchaseInvoice.Inventory += lastProductInventory;

            var newProductInPurchaseInvoice = _productRepository.FindById(dto.ProductId);
            newProductInPurchaseInvoice.Inventory -= dto.Number;
        }

        private static void UpdateSalesInvoiceByDto(UpdateSalesInvoiceDto dto, SalesInvoice purchaseInvoice)
        {
            purchaseInvoice.CodeOfProduct = dto.CodeOfProduct;
            purchaseInvoice.NameOfProduct = dto.NameOfProduct;
            purchaseInvoice.Number = dto.Number;
            purchaseInvoice.TotalCost = dto.TotalCost;
            purchaseInvoice.Date = dto.Date;
            purchaseInvoice.ProductId = dto.ProductId;
        }
    }
}
