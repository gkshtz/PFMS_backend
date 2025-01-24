using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class BudgetsRepository<Dto, Entity>: GenericRepository<Dto, Entity>, IBudgetsRepository<Dto>
        where Dto: BudgetDto
        where Entity: Budget
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbcontext;
        public BudgetsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbcontext = appDbContext;
            _mapper = mapper;
        }

        public async Task<BudgetDto?> GetBudgetByUserId(Guid userId, int month, int year)
        {
            var budget = await _appDbcontext.Budgets.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.Month == month && x.Year == year);
            return _mapper.Map<BudgetDto?>(budget);
        }
    }
}
