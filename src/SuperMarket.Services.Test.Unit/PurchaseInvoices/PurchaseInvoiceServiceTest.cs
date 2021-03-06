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
using SuperMarket.Services.PurchaseInvoices.Exceptions;
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

        [Fact]
        public void Update_updates_a_purchaseInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product1 = ProductFactory.CreatProduct("1", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product1));

            var product2 = ProductFactory.CreatProduct("2", 8, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product2));

            var purchaseInvoice = PurchaseInvoiceFactory.
               CreateSalesInvoice(product1.Code, product1.Name, product1.Id);
            _dataContext.Manipulate(_ => _.PurchaseInvoices.Add(purchaseInvoice));

            UpdatePurchaseInvoiceDto dto = GeneratePurchaseInvoiceDto(product2);
            _sut.Update(dto, purchaseInvoice.Id);

            var expected = _dataContext.PurchaseInvoices.FirstOrDefault();
            expected.CodeOfProduct.Should().Be(dto.CodeOfProduct);
            product1.Inventory.Should().Be(8);
            product2.Inventory.Should().Be(10);
        }

        [Fact]
        public void Throw_exception_if_PurchaseInvoiceDoesNotExistException_when_update_a_PurchaseInvoice_that_does_not_exist_in_database()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product2 = ProductFactory.CreatProduct("2", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product2));

            var dto = GeneratePurchaseInvoiceDto(product2);

            var fakeId = 100;

            Action expected = () => _sut.Update(dto, fakeId);
            expected.Should().ThrowExactly<PurchaseInvoiceDoesNotExistException>();
        }

        [Fact]
        public void Delete_deletes_a_PurchaseInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = ProductFactory.CreatProduct("1", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product));

            var purchaseInvoice = PurchaseInvoiceFactory.
                CreateSalesInvoice(product.Code, product.Name, product.Id);
            _dataContext.Manipulate(_ => _.PurchaseInvoices.Add(purchaseInvoice));

            _sut.Delete(purchaseInvoice.Id);

            _dataContext.SalesInvoices.Should().HaveCount(0);
            product.Inventory.Should().Be(8);
        }

        [Fact]
        public void Throw_exception_if_PurchaseInvoiceDoesNotExistException_when_Delete_a_PurchaseInvoice_that_does_not_exist_in_database()
        {
            var fakeId = 100;

            Action expected = () => _sut.Delete(fakeId);
            expected.Should().ThrowExactly<PurchaseInvoiceDoesNotExistException>();
        }

        [Fact]
        public void GetAll_returns_all_PurchaseInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product1 = ProductFactory.CreatProduct("1", 10, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product1));

            var product2 = ProductFactory.CreatProduct("1", 5, category.Id);
            _dataContext.Manipulate(_ => _.products.Add(product2));

            Generate_a_list_of_purchase_invoices(
               product1.Code, product1.Name, product1.Id,
               product2.Code, product2.Name, product2.Id);

            var expected = _sut.GetAll();

            expected.Should().HaveCount(2);
        }

        private UpdatePurchaseInvoiceDto GeneratePurchaseInvoiceDto(Product product2)
        {
            var dto = new UpdatePurchaseInvoiceDto
            {
                CodeOfProduct = product2.Code,
                NameOfProduct = product2.Name,
                Number = 2,
                Price = 7000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = product2.Id,
            };
            return dto;
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

        private void Generate_a_list_of_purchase_invoices(
           string productCode1, string productName1, int productId1,
            string productCode2, string productName2, int productId2)
        {
            var purchaseInvoices = new List<PurchaseInvoice>
            {
                new PurchaseInvoice
                {
                CodeOfProduct = productCode1,
                NameOfProduct = productName1,
                Number = 2,
                Price = 10000,
                Date = new DateTime(05 / 02 / 2022),
                ProductId = productId1,
                },

                new PurchaseInvoice
                {
                CodeOfProduct = productCode2,
                NameOfProduct = productName2,
                Number = 2,
                Price = 10000,
                Date = new DateTime(07 / 02 / 2022),
                ProductId = productId2,
                }
            };
            _dataContext.Manipulate(_ => _.PurchaseInvoices.AddRange(purchaseInvoices));
        }

    }
}
