using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.Products.Exceptions;
using SuperMarket.Services.SalesInvoices.Contracts;
using System.Collections.Generic;

namespace SuperMarket.Services.Products
{
    public class ProductAppService : ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly SalesInvoiceRepository _salesInvoiceRepository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(ProductRepository productRepository,
            SalesInvoiceRepository salesInvoiceRepository , UnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _salesInvoiceRepository = salesInvoiceRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddProductDto dto)
        {
            Product product = CreateProduct(dto);

            var isProductExist = _productRepository.
                IsProductCodeExist(dto.Code);
            if (isProductExist)
            {
                throw new ProductISAlreadyExistException();
            }

            _productRepository.Add(product);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var product = _productRepository.FindById(id);

            var IsProductUsed = _salesInvoiceRepository.
                IsProductUsedInSalesInvoice(id);
            if (IsProductUsed)
            {
                throw new ProductUsedInSalesInvoiceException();
            }

            if(product == null)
            {
                throw new ProductDoesNotExistException();
            }

            _productRepository.Delete(product);
            _unitOfWork.Commit();
        }

        public List<GetProductDto> Get(int categoryId)
        {
            return _productRepository.Get(categoryId);
        }

        public List<GetProductDto> GetAll()
        {
            return _productRepository.GetAll();
        }

        public void Update(UpdateProductDto dto, int id)
        {
            var product = _productRepository.FindById(id);

            var isCodeExist = _productRepository.IsProductCodeExist(dto.Code);
            if (isCodeExist)
            {
                throw new ProductCodeIsAlreadyExistException();
            }

            if (product == null)
            {
                throw new ProductDoesNotExistException();
            }

            UpdateProductByDto(dto, product);

            _unitOfWork.Commit();
        }

        private static void UpdateProductByDto(UpdateProductDto dto, Product product)
        {
            product.Code = dto.Code;
            product.Name = dto.Name;
            product.MinimumInventory = dto.MinimumInventory;
            product.Price = dto.Price;
            product.Inventory = dto.Inventory;
            product.CategoryId = dto.CategoryId;
        }

        private static Product CreateProduct(AddProductDto dto)
        {
            return new Product
            {
                Code = dto.Code,
                Name = dto.Name,
                MinimumInventory = dto.MinimumInventory,
                Price = dto.Price,
                Inventory = dto.Inventory,
                CategoryId = dto.CategoryId,
            };
        }

    }
}
