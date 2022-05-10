using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.PurchaseInvoices.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/purchaseInvoice")]
    [ApiController]
    public class PurchaseInvoicesController : Controller
    {
        private readonly PurchaseInvoiceService _service;

        public PurchaseInvoicesController(PurchaseInvoiceService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddPurchaseInvoiceDto dto)
        {
             _service.Add(dto);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpGet]
        public List<GetPurchaseInvoiceDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPatch("{id}")]
        public void Update(UpdatePurchaseInvoiceDto dto, [FromRoute]int id)
        {
            _service.Update(dto, id);
        }
    }
}
