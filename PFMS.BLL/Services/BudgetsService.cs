using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class BudgetsService: IBudgetsService
    {
        private readonly IBudgetsRepository _budgetRepository;
        private readonly IMapper _mapper;
        public BudgetsService(IBudgetsRepository budgetRepository, IMapper mapper)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
        }
        public async Task AddNewBudget(BudgetBo budgetBo, Guid userId)
        {
            budgetBo.UserId = userId;
            budgetBo.BudgetId = Guid.NewGuid();
            var budgetDto = _mapper.Map<BudgetDto>(budgetBo);

            await _budgetRepository.AddBudget(budgetDto);
        }
    }
}
