using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Persistance.EF.SalesInvoices;
using SuperMarket.Services.SalesInvoices;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Specs.Infrastructure;
using SuperMarket.Test.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.SalesInvoices
{
    [Scenario("نمایش فاکتور فروش")]
    [Feature("",
          AsA = "فروشنده ",
          IWantTo = "فاکتور فروش را مشاهده کنم",
          InOrderTo = " فروش براساس دسته بندی و کالا را مشاهده کنم"
          )]
    public class GetSalesInvoiceByProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SalesInvoiceRepository _ripository;
        private readonly SalesInvoiceService _sut;
        private Category _category1;
        private Category _category2;
        private List<Product> products;
        private List<SalesInvoice> _salesInvoices;
        private List<GetSalesInvoiceDto> _expected;
        public GetSalesInvoiceByProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _ripository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_ripository, _unitOfWork);
        }
    }
}
