using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Products.Cantracts
{
    public interface ProductService
    {
        void Add(AddProductDto dto);
        void Update(UpdateProductDto dto, int id);
        List<GetProductDto> Get(int categoryId);
        List<GetProductDto> GetAll();
    }
}
