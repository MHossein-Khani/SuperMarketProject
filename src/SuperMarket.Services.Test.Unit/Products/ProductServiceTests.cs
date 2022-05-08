using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Services.Products;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.Products.Exceptions;
using SuperMarket.Test.Tools;
using System;
using System.Collections.Generic;
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

            var product = ProductFactory.CreatProduct("1", 10, category.Id);
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

            var product = ProductFactory.CreatProduct("1", 10, category.Id);
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

        [Fact]
        public void Throw_Exception_if_CategoryCodeIsAlreadyExistException_when_updete_a_product_to_a_duplicate_code()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product1 = ProductFactory.CreatProduct("1", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product1));

            var product2 = ProductFactory.CreatProduct("2", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product2));

            var dto = GenerateUpdateProductDto(category.Id);
            Action expected = () => _sut.Update(dto, category.Id);
            expected.Should().ThrowExactly<CategoryCodeIsAlreadyExistException>();
        }

        [Fact]
        public void Get_returns_products_by_category_id_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            Create_list_of_products_by_category_id(category.Id);

            var dto = new GetProductDto
            {
                CategoryId = category.Id
            };
            var expected = _sut.Get(dto.CategoryId);

            expected.Should().HaveCount(2);
        }

        [Fact]
        public void GetAll_returns_all_products_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var category2 = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(category2));

            Create_list_of_products(category.Id, category2.Id);


            var expected = _sut.GetAll();

            expected.Should().HaveCount(2);
        }

        [Fact]
        public void Delete_deletes_product_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            _sut.Delete(product.Id);

            _dataContext.products.Should().HaveCount(0);
        }

        [Fact]
        public void Throw_Exception_if_ProductUsedInSalesInvoiceException_when_deleting_a_product_that_sold_before()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            var salesInvoice = SalesInvoiceFactory.
                CreateSalesInvoice(product.Code, product.Name, product.Id);
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(salesInvoice));

            Action expected = () => _sut.Delete(product.Id);
            expected.Should().ThrowExactly<ProductUsedInSalesInvoiceException>();
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

        private void Create_list_of_products_by_category_id(int categoryId)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Code = "1",
                    Name = "شیر کاله",
                    MinimumInventory = 5,
                    Price = 5000,
                    Inventory = 10,
                    CategoryId = categoryId
                },

                new Product
                {
                    Code = "2",
                    Name = "test",
                    MinimumInventory = 8,
                    Price = 10000,
                    Inventory = 9,
                    CategoryId = categoryId
                },
            };

            _dataContext.Manipulate(_ => _.products.AddRange(products));
        }

        private void Create_list_of_products(int categoryId1, int categoryId2)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Code = "1",
                    Name = "شیر کاله",
                    MinimumInventory = 5,
                    Price = 5000,
                    Inventory = 10,
                    CategoryId = categoryId1
                },

                new Product
                {
                    Code = "2",
                    Name = "test",
                    MinimumInventory = 8,
                    Price = 10000,
                    Inventory = 9,
                    CategoryId = categoryId2
                },
            };

            _dataContext.Manipulate(_ => _.products.AddRange(products));
        }

    }
}
