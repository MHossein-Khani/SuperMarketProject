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


    }
}
