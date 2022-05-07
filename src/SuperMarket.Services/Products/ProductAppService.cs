using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Products.Cantracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            _productRepository.Add(product);
            _unitOfWork.Commit();
        }
    }
}
