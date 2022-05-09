using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Categories;
using SuperMarket.Persistance.EF.SalesInvoices;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using SuperMarket.Services.SalesInvoices;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Services.SalesInvoices.Exceptions;
using SuperMarket.Test.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SuperMarket.Services.Test.Unit.SalesInvoices
{
    public class SalesInvoiceServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly SalesInvoiceService _sut;

        public SalesInvoiceServiceTests()
        {
            _dataContext = new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_SalesInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 10,category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            AddSalesInvoiceDto dto = GenerateAddProductDto(product);
            _sut.Add(dto);

            var expected = _dataContext.SalesInvoices.FirstOrDefault();
            expected.Should().NotBeNull();
            expected.CodeOfProduct.Should().Be(dto.CodeOfProduct);
            expected.NameOfProduct.Should().Be(dto.NameOfProduct);
            expected.Number.Should().Be(dto.Number);
            expected.Date.Should().Be(dto.Date);
            expected.ProductId.Should().Be(dto.ProductId);
            product.Inventory.Should().Be(8);
        }

        [Fact]
        public void Throw_exception_if_ProductInventoryIsFinishedException_when_add_sales_invoice_that_the_inventory_of_product_is_zero()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 0, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            AddSalesInvoiceDto dto = GenerateAddProductDto(product);

            Action expected = () => _sut.Add(dto);
            _dataContext.SalesInvoices.Should().HaveCount(0);
            expected.Should().ThrowExactly<TheNumberOfProductsIsLessThanTheNumberRequestedException>();
        }

        [Fact]
        public void Update_updates_a_salesInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product1 = ProductFactory.CreatProduct("1", 8, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product1));

            var product2 = ProductFactory.CreatProduct("2", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product2));

            var salesInvoice = SalesInvoiceFactory.
               CreateSalesInvoice(product1.Code, product1.Name, product1.Id);
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(salesInvoice));

            UpdateSalesInvoiceDto dto = GenerateUpdateSalesInvoiceDto(product2);
            _sut.Update(dto, salesInvoice.Id);

            var expected = _dataContext.SalesInvoices.FirstOrDefault();
            expected.CodeOfProduct.Should().Be(dto.CodeOfProduct);
            product1.Inventory.Should().Be(10);
            product2.Inventory.Should().Be(8);
        }

        [Fact]
        public void Throw_exception_if_InventoryIsOutOfStockException_when_updating_a_salesInvoice_to_another_product()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product1 = ProductFactory.CreatProduct("1", 8, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product1));

            var product2 = ProductFactory.CreatProduct("2", 0, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product2));

            var salesInvoice = SalesInvoiceFactory.
               CreateSalesInvoice(product1.Code, product1.Name, product1.Id);
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(salesInvoice));

            UpdateSalesInvoiceDto dto = GenerateUpdateSalesInvoiceDto(product2);

            Action expected = () => _sut.Update(dto, salesInvoice.Id);
            expected.Should().ThrowExactly<InventoryIsOutOfStockException>();
            var expectedSalesInvoice = _dataContext.SalesInvoices.FirstOrDefault();
            expectedSalesInvoice.CodeOfProduct.Should().Be(product1.Code);
        }

        [Fact]
        public void Delete_deletes_a_salesInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 8, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            var salesInvoice = SalesInvoiceFactory.
               CreateSalesInvoice(product.Code, product.Name, product.Id);
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(salesInvoice));

            _sut.Delete(salesInvoice.Id);

            _dataContext.SalesInvoices.Should().HaveCount(0);
            product.Inventory.Should().Be(10);
        }

        [Fact]
        public void GetByCategory_returns_all_salesInvoices_that_have_the_same_category()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product1 = ProductFactory.CreatProduct("1", 8, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product1));

            var product2 = ProductFactory.CreatProduct("2", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product2));

            Generate_a_list_of_salesInvoice
                (product1.Code, product1.Name, product1.Id,
                product2.Code, product2.Name, product2.Id);

            var expected = _sut.GetByCategory(category.Id);
            expected.Should().HaveCount(2);
        }

        [Fact]
        public void GetByProduct_returns_all_salesInvoices_that_have_same_product()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 8, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            Create_a_list_of_sales_invoice(product.Code, product.Name, product.Id);

            var expected = _sut.GetByProduct(product.Id);
            expected.Should().HaveCount(2);
        }



        private static UpdateSalesInvoiceDto GenerateUpdateSalesInvoiceDto(Product product2)
        {
            return new UpdateSalesInvoiceDto
            {
                CodeOfProduct = product2.Code,
                NameOfProduct = product2.Name,
                Number = 2,
                TotalCost = 10000,
                Date = new DateTime(05 / 02 / 2020),
                ProductId = product2.Id,
            };
        }

        private static AddSalesInvoiceDto GenerateAddProductDto(Product product)
        {
            return new AddSalesInvoiceDto
            {
                CodeOfProduct = product.Code,
                NameOfProduct = product.Name,
                Number = 2,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = product.Id,
            };
        }

        private void Generate_a_list_of_salesInvoice
            (string productCode1, string productName1,int productId1,
            string productCode2, string productName2, int productId2)
        {
            var salesInvoices = new List<SalesInvoice>()
            {
                new SalesInvoice()
                {
                    CodeOfProduct = productCode1,
                    NameOfProduct = productName1,
                    Number = 2,
                    TotalCost = 10000,
                    Date = new DateTime(05/02/2022),
                    ProductId = productId1,
                },
                new SalesInvoice()
                {
                    CodeOfProduct = productCode2,
                    NameOfProduct = productName2,
                    Number = 2,
                    TotalCost = 15000,
                    Date = new DateTime(05/02/2022),
                    ProductId = productId2,
                }
            };
            _dataContext.Manipulate(_ => _.SalesInvoices.AddRange(salesInvoices));
        }

        private void Create_a_list_of_sales_invoice(string productCode,
            string productName, int productId)
        {
            var salesInvoice = new List<SalesInvoice>
            {
                new SalesInvoice
                {
                CodeOfProduct = productCode,
                NameOfProduct = productName,
                Number = 2,
                TotalCost = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = productId,
                },

                new SalesInvoice
                {
                CodeOfProduct = productCode,
                NameOfProduct = productName,
                Number = 4,
                TotalCost = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = productId,
                },
            };
            _dataContext.Manipulate(_ => _.SalesInvoices.AddRange(salesInvoice));
        }

    }
}
