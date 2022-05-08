using FluentAssertions;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.Products.Exceptions;
using SuperMarket.Test.Tools;
using System;
using System.Linq;
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

        [Fact]
        public void Throw_exception_if_ProductISAlreadyExistException_when_adding_a_product_that_has_duplicate_code()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct(category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            var dto = GenerateAddProductDto(category.Id);

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<ProductISAlreadyExistException>();
        }

        [Fact]
        public void Update_updates_a_product_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct(category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            var dto = GenerateUpdateProductDto(category.Id);
            _sut.Update(dto, product.Id);

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

        private static UpdateProductDto GenerateUpdateProductDto(int categoryId)
        {
            return new UpdateProductDto
            {
                Code = "2",
                Name = "ماست سون",
                MinimumInventory = 2,
                Price = 7000,
                Inventory = 5,
                CategoryId = categoryId
            };
        }
    }
}
