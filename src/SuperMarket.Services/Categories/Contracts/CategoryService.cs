﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Categories.Contracts
{
    public interface CategoryService
    {
        void Add(AddCategoryDto dto);
        void Update(UpdateCategoryDto dto, int id);
        List<GetCategoryDto> GetAll();
        void Delete(int id);
    }
}
