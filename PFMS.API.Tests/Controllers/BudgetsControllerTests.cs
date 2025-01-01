using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PFMS.API.Controllers;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;

namespace PFMS.API.Tests.Controllers
{
    public class BudgetsControllerTests
    {
        [Fact]
        public async void Add_New_Budget_Successfully_Added()
        {
            //Arrange
            Mock<IBudgetsService> budgetService = new Mock<IBudgetsService>();
            Mock<IMapper> mapper = new Mock<IMapper>();

            var budgetsController = new BudgetsController(budgetService.Object, mapper.Object);
            var budgetModel = new BudgetRequestModel()
            {
                BudgetAmount = 1000,
                Month = 1,
                Year = 2025,
            };

            var budgetBo = new BudgetBo()
            {
                BudgetAmount = 1000,
                Month = 1,
                Year = 2025
            };

            budgetService.Setup(x => x.AddNewBudget(It.IsAny<BudgetBo>(), It.IsAny<Guid>()));
            mapper.Setup(x => x.Map<BudgetBo>(It.IsAny<BudgetRequestModel>())).Returns(budgetBo);

            //Act
            var response = await budgetsController.AddAsync(budgetModel);

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.NotNull(successResponse);

            budgetService.Verify(x=>x.AddNewBudget(budgetBo, It.IsAny<Guid>()), Times.Once);
        }
    }
}
