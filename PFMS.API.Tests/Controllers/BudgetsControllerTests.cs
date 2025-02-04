﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PFMS.API.Controllers;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

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

            var createdResult = response as CreatedResult;
            Assert.NotNull(createdResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(createdResult.Value);
            Assert.Equal(201, successResponse.StatusCode);
            Assert.True(successResponse.ResponseData);

            budgetService.Verify(x=>x.AddNewBudget(budgetBo, It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void Get_Budget_Successfully_Fetched()
        {
            //Arrange
            Mock<IBudgetsService> budgetService = new Mock<IBudgetsService>();
            Mock<IMapper> mapper = new Mock<IMapper>();

            var budgetsController = new BudgetsController(budgetService.Object, mapper.Object);

            var budgetResponseModel = new BudgetResponseModel()
            {
                Id = Guid.NewGuid(),
                BudgetAmount = 1000,
                SpentPercentage = 30
            };
            var budgetBo = new BudgetBo()
            {
                Id = Guid.NewGuid(),
                BudgetAmount = 1000,
                SpentPercentage = 30
            };

            budgetService.Setup(x => x.GetBudget(It.IsAny<Guid>(), 1, 2025)).ReturnsAsync(budgetBo);
            mapper.Setup(x => x.Map<BudgetResponseModel>(It.IsAny<BudgetBo>())).Returns(budgetResponseModel);

            //Act
            var response = await budgetsController.GetBudgetAsync(1, 2025);

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<BudgetResponseModel>>(okResult.Value);

            budgetService.Verify(x => x.GetBudget(It.IsAny<Guid>(), 1, 2025), Times.Once);
            Assert.IsType<BudgetResponseModel>(successResponse.ResponseData);
            Assert.Equal(200, successResponse.StatusCode);
        }

        [Fact]
        public async void Update_Budget_Successfully_Updated()
        {
            //Arrange
            Mock<IBudgetsService> budgetService = new Mock<IBudgetsService>();
            Mock<IMapper> mapper = new Mock<IMapper>();

            var budgetsController = new BudgetsController(budgetService.Object, mapper.Object);

            var budgetRequestModel = new BudgetRequestModel()
            {
                BudgetAmount = 2000,
                Month = 1,
                Year = 2025
            };
            var budgetBo = new BudgetBo()
            {
                BudgetAmount = 2000,
                Month = 1,
                Year = 2025
            };

            budgetService.Setup(x => x.UpdateBudget(It.IsAny<BudgetBo>(), It.IsAny<Guid>(), It.IsAny<Guid>()));
            mapper.Setup(x => x.Map<BudgetBo>(It.IsAny<BudgetRequestModel>())).Returns(budgetBo);

            //Act
            var response = await budgetsController.UpdateBudgetAsync(budgetRequestModel, Guid.NewGuid());

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.True(successResponse.ResponseData);
            Assert.Equal(200, successResponse.StatusCode);

            budgetService.Verify(x => x.UpdateBudget(budgetBo, It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void Delete_Budget_Successful_Delete_Test()
        {
            //Arrange
            Mock<IBudgetsService> budgetsService = new Mock<IBudgetsService>();
            Mock<IMapper> mapper = new Mock<IMapper>();

            var budgetsController = new BudgetsController(budgetsService.Object, mapper.Object);
            
            var budgetId = Guid.Parse("65c6f032-c1b8-4b2b-b437-6a45e63b2143");
            var userId = Guid.Parse("f23ef035-60fb-4e17-86b0-05d5d2084bc2");

            budgetsService.Setup(x => x.DeleteBudget(It.IsAny<Guid>(), It.IsAny<Guid>()));

            //Act
            var response = await budgetsController.DeleteBudgetAsync(budgetId);

            //Assert
            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.Equal(200, successResponse.StatusCode);
            Assert.True(successResponse.ResponseData);
            Assert.Equal(ResponseMessage.Success.ToString(), successResponse.ResponseMessage);

            budgetsService.Verify(x=>x.DeleteBudget(budgetId, It.IsAny<Guid>()), Times.Once);
        }
    }
}
