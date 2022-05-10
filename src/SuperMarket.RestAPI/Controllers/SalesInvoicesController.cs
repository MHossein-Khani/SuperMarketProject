using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.SalesInvoices;
using SuperMarket.Services.SalesInvoices.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/salesInvoices")]
    [ApiController]
    public class SalesInvoicesController : Controller
    {
        private readonly SalesInvoiceService _service;

        public SalesInvoicesController(SalesInvoiceService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddSalesInvoiceDto dto)
        {
            _service.Add(dto);
        }

        [HttpPatch("{id}")]
        public void Delete([FromRoute]int id)
        {
            _service.Delete(id);
        }

        [HttpGet]
        public List<GetSalesInvoiceDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{categoryId}")]
        public List<GetSalesInvoiceDto> GetByCategory([FromRoute]int categoryId)
        {
            return _service.GetByCategory(categoryId);
        }

        [HttpGet("{productId}")]
        public List<GetSalesInvoiceDto> GetByProduct(int productId)
        {
            return _service.GetByProduct(productId);
        }

        [HttpPatch("{id}")]
        public void Update(UpdateSalesInvoiceDto dto, int id)
        {
            _service.Update(dto, id);
        }
    }
}
