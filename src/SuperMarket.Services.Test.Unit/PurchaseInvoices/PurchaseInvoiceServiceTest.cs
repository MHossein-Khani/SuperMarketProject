using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.Products;
using SuperMarket.Persistance.EF.PurchaseInvoices;
using SuperMarket.Services.Products.Cantracts;
using SuperMarket.Services.PurchaseInvoices;
using SuperMarket.Services.PurchaseInvoices.Contracts;
using SuperMarket.Test.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperMarket.Services.Test.Unit.PurchaseInvoices
{
    public class PurchaseInvoiceServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly PurchaseInvoiceRepository _ripository;
        private readonly ProductRepository _productRepository;
        private readonly PurchaseInvoiceService _sut;

        public PurchaseInvoiceServiceTest()
        {
            _dataContext = new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFPurchaseInvoiceRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_ripository, _productRepository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_SalesInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            AddPurchaseInvoiceDto dto = GenerateAddPurchaseInvoiceDto(product);
            _sut.Add(dto);

            var expected = _dataContext.PurchaseInvoices.FirstOrDefault();
            expected.Should().NotBeNull();
            expected.CodeOfProduct.Should().Be(dto.CodeOfProduct);
            expected.NameOfProduct.Should().Be(dto.NameOfProduct);
            expected.Number.Should().Be(dto.Number);
            expected.Price.Should().Be(dto.Price);
            expected.Date.Should().Be(dto.Date);
            expected.ProductId.Should().Be(dto.ProductId);
            product.Inventory.Should().Be(12);
        }


        private static AddPurchaseInvoiceDto GenerateAddPurchaseInvoiceDto(Product product)
        {
            return new AddPurchaseInvoiceDto
            {
                CodeOfProduct = product.Code,
                NameOfProduct = product.Name,
                Number = 2,
                Price = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = product.Id,
            };
        }
    }
}
