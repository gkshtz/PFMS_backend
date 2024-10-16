﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId);

        public Task AddCategory(TransactionCategoryDto categoryDto);

        public Task AddCategoryToUser(CategoryToUserDto categoryToUserDto);
    }
}
