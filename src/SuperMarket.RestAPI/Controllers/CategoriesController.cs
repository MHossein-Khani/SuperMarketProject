using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.Categories.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/catgeories")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly CategoryService _service;

        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _service.Add(dto);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpGet]
        public List<GetCategoryDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPatch("{id}")]
        public void Update(UpdateCategoryDto dto, [FromRoute]int id)
        {
            _service.Update(dto, id);
        }


    }
}
