﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<TransactionCategoryBo>> GetAllCategories(Guid userId);

        public Task AddCategory(TransactionCategoryBo categoryBo, Guid userId);

        public Task DeleteCategory(Guid categoryId, Guid userId);
    }
}
