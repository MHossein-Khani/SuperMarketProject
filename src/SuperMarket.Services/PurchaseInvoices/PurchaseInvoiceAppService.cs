using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.PurchaseInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseInvoices
{
    public class PurchaseInvoiceAppService : PurchaseInvoiceService
    {
        private readonly PurchaseInvoiceRepository _repository;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public PurchaseInvoiceAppService(PurchaseInvoiceRepository repository,
            ProductRepository productRepository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddPurchaseInvoiceDto dto)
        {
            var purchaseInvoice = new PurchaseInvoice
            {
                CodeOfProduct = dto.CodeOfProduct,
                NameOfProduct = dto.NameOfProduct,
                Number = dto.Number,
                Price = dto.Price,
                Date = dto.Date,
                ProductId = dto.ProductId,
            };

            var newProductInPurchaseInvoice = _productRepository.FindById(dto.ProductId);
            newProductInPurchaseInvoice.Inventory += dto.Number;

            _repository.Add(purchaseInvoice);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var purchaseInvoice = _repository.FindById(id);
            var lastProductId = purchaseInvoice.ProductId;
            var lastProductInventory = purchaseInvoice.Number;

            _repository.Delete(purchaseInvoice);

            var lastProductInSalesInvoice = _productRepository.FindById(lastProductId);
            lastProductInSalesInvoice.Inventory -= lastProductInventory;

            _unitOfWork.Commit();
        }

        public List<GetPurchaseInvoiceDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(UpdatePurchaseInvoiceDto dto, int id)
        {
            var purchaseInvoice = _repository.FindById(id);

            var lastProductId = purchaseInvoice.ProductId;
            var lastProductInventory = purchaseInvoice.Number;

            purchaseInvoice.CodeOfProduct = dto.CodeOfProduct;
            purchaseInvoice.NameOfProduct = dto.NameOfProduct;
            purchaseInvoice.Number = dto.Number;
            purchaseInvoice.Price = dto.Price;
            purchaseInvoice.Date = dto.Date;
            purchaseInvoice.ProductId = dto.ProductId;

            var lastProductInSalesInvoice = _productRepository.FindById(lastProductId);
            lastProductInSalesInvoice.Inventory -= lastProductInventory;

            var newProductInSalesInvoice = _productRepository.FindById(dto.ProductId);
            newProductInSalesInvoice.Inventory += dto.Number;

            _unitOfWork.Commit();
        }
    }
}
