using FluentAssertions;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Test.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Products
{
    public class ProductServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRipository;
        private readonly ProductService _sut;

        public ProductServiceTests()
        {
            _dataContext = new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _productRipository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_productRipository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_product_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = GenerateAddProductDto(category.Id);
            _sut.Add(dto);

            var expected = _dataContext.products.FirstOrDefault();
            expected.Code.Should().Be(dto.Code);
            expected.Name.Should().Be(dto.Name);
            expected.MinimumInventory.Should().Be(dto.MinimumInventory);
            expected.Price.Should().Be(dto.Price);
            expected.Inventory.Should().Be(dto.Inventory);
            expected.CategoryId.Should().Be(dto.CategoryId);

        }

       


        private static AddProductDto GenerateAddProductDto(int categoryId)
        {
            return new AddProductDto
            {
                Code = "1",
                Name = "شیر کاله",
                MinimumInventory = 5,
                Price = 5000,
                Inventory = 10,
                CategoryId = categoryId
            };
        }
    }
}
