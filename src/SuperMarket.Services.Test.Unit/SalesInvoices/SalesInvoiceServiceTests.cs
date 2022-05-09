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

            product.Inventory -= dto.Number;
            _dataContext.Manipulate(_ => _.products.Update(product));

            var expectedProduct = _dataContext.products.FirstOrDefault();
            expectedProduct.Inventory.Should().Be(8);
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

            product2.Inventory -= dto.Number;
            _dataContext.Manipulate(_ => _.products.Update(product2));

            var expectedProduct = _dataContext.products.
                FirstOrDefault(p => p.Id == product2.Id);
            expectedProduct.Inventory.Should().Be(8);
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
    }
}
