using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.Products.Exceptions;
using System.Collections.Generic;

namespace SuperMarket.Services.Products
{
    public class ProductAppService : ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(ProductRepository productRepository, UnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddProductDto dto)
        {
            var product = new Product
            {
                Code = dto.Code,
                Name = dto.Name,
                MinimumInventory = dto.MinimumInventory,
                Price = dto.Price,
                Inventory = dto.Inventory,
                CategoryId = dto.CategoryId,
            };

            var isProductExist = _productRepository.
                IsProductCodeExist(dto.Code);
            if (isProductExist)
            {
                throw new ProductISAlreadyExistException();
            }

            _productRepository.Add(product);
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
                throw new CategoryCodeIsAlreadyExistException();
            }

            product.Code = dto.Code;
            product.Name = dto.Name;
            product.MinimumInventory = dto.MinimumInventory;
            product.Price = dto.Price;
            product.Inventory = dto.Inventory;
            product.CategoryId = dto.CategoryId;

            _unitOfWork.Commit();
        }
    }
}
