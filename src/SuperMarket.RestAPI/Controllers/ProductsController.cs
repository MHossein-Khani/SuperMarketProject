using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.Products.Cantracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductService _service;

        public ProductsController(ProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddProductDto dto)
        {
            _service.Add(dto);
        }

        [HttpDelete("{id}")]
        public void Delete([FromRoute]int id)
        {
            _service.Delete(id);
        }

        [HttpGet("{categoryId}")]
        public List<GetProductDto> Get([FromRoute]int categoryId)
        {
            return _service.Get(categoryId);
        }

        [HttpGet]
        public List<GetProductDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPatch("{id}")]
        public void Update(UpdateProductDto dto, [FromRoute]int id)
        {
            _service.Update(dto, id);
        }
    }
}
