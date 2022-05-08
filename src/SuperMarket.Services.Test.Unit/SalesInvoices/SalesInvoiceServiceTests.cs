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

            var product = ProductFactory.CreatProduct("1", category.Id);
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
