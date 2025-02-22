using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PFMS.API.Controllers;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Tests.Controllers
{
    public class BudgetsControllerSetup
    {
        public Mock<IBudgetsService> BudgetsService { get; }
        public Mock<IMapper> Mapper;
        public BudgetsController BudgetsController { get; }

        public BudgetBo BudgetBo { get; }
        public BudgetRequestModel BudgetModel { get; }
        
        public BudgetsControllerSetup()
        {
            BudgetsService = new Mock<IBudgetsService>();
            Mapper = new Mock<IMapper>();
            BudgetsController = new BudgetsController(BudgetsService.Object, Mapper.Object);

            BudgetBo = new BudgetBo()
            {
                BudgetAmount = 1000,
                Month = 1,
                Year = 2025
            };
            BudgetModel = new BudgetRequestModel()
            {
                BudgetAmount = 1000,
                Month = 1,
                Year = 2025
            };
        }
    }

    public class BudgetsControllerTests: IClassFixture<BudgetsControllerSetup>
    {
        private readonly BudgetsControllerSetup _setup;
        public BudgetsControllerTests(BudgetsControllerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public async void Add_New_Budget_Successfully_Added()
        {
            //Arrange
            _setup.BudgetsService.Setup(x => x.AddNewBudget(It.IsAny<BudgetBo>(), It.IsAny<Guid>()));
            _setup.Mapper.Setup(x => x.Map<BudgetBo>(It.IsAny<BudgetRequestModel>())).Returns(_setup.BudgetBo);

            //Act
            var response = await _setup.BudgetsController.AddAsync(_setup.BudgetModel);

            //Assert
            Assert.NotNull(response);

            var createdResult = response as CreatedResult;
            Assert.NotNull(createdResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(createdResult.Value);
            Assert.Equal(201, successResponse.StatusCode);
            Assert.True(successResponse.ResponseData);

            _setup.BudgetsService.Verify(x=>x.AddNewBudget(_setup.BudgetBo, It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void Get_Budget_Successfully_Fetched()
        {
            //Arrange
            var budgetResponseModel = new BudgetResponseModel()
            {
                Id = Guid.NewGuid(),
                BudgetAmount = 1000,
                SpentPercentage = 30
            };

            _setup.BudgetsService.Setup(x => x.GetBudget(It.IsAny<Guid>(), 1, 2025)).ReturnsAsync(_setup.BudgetBo);
            _setup.Mapper.Setup(x => x.Map<BudgetResponseModel>(It.IsAny<BudgetBo>())).Returns(budgetResponseModel);

            //Act
            var response = await _setup.BudgetsController.GetBudgetAsync(1, 2025);

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<BudgetResponseModel>>(okResult.Value);

            _setup.BudgetsService.Verify(x => x.GetBudget(It.IsAny<Guid>(), 1, 2025), Times.Once);
            Assert.IsType<BudgetResponseModel>(successResponse.ResponseData);
            Assert.Equal(200, successResponse.StatusCode);
        }

        [Fact]
        public async void Update_Budget_Successfully_Updated()
        {
            //Arrange
            _setup.BudgetsService.Setup(x => x.UpdateBudget(It.IsAny<BudgetBo>(), It.IsAny<Guid>(), It.IsAny<Guid>()));
            _setup.Mapper.Setup(x => x.Map<BudgetBo>(It.IsAny<BudgetRequestModel>())).Returns(_setup.BudgetBo);

            //Act
            var response = await _setup.BudgetsController.UpdateBudgetAsync(_setup.BudgetModel, Guid.NewGuid());

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.True(successResponse.ResponseData);
            Assert.Equal(200, successResponse.StatusCode);

            _setup.BudgetsService.Verify(x => x.UpdateBudget(_setup.BudgetBo, It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void Delete_Budget_Successful_Delete_Test()
        {
            //Arrange         
            var budgetId = Guid.Parse("65c6f032-c1b8-4b2b-b437-6a45e63b2143");
            var userId = Guid.Parse("f23ef035-60fb-4e17-86b0-05d5d2084bc2");

            _setup.BudgetsService.Setup(x => x.DeleteBudget(It.IsAny<Guid>(), It.IsAny<Guid>()));

            //Act
            var response = await _setup.BudgetsController.DeleteBudgetAsync(budgetId);

            //Assert
            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.Equal(200, successResponse.StatusCode);
            Assert.True(successResponse.ResponseData);
            Assert.Equal(ResponseMessage.Success.ToString(), successResponse.ResponseMessage);

            _setup.BudgetsService.Verify(x=>x.DeleteBudget(budgetId, It.IsAny<Guid>()), Times.Once);
        }
    }
}
