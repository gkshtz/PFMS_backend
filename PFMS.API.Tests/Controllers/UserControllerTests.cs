using System.Security.Cryptography.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Moq;
using PFMS.API.Controllers;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async void Gell_All_Users_When_Users_List_Is_Returned_Test()
        {
            // Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            Mock<IMapper> mapper = new Mock<IMapper>(); 
            
            UserController userController = new UserController(userService.Object, mapper.Object);

            userService.Setup(x => x.GetAllUsers()).ReturnsAsync(new List<UserBo>());
            mapper.Setup(x => x.Map<List<UserResponseModel>>(It.IsAny<List<UserBo>>())).Returns(new List<UserResponseModel>());

            //Act
            var response = await userController.GetAllAsync();

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            Assert.IsType<GenericSuccessResponse<List<UserResponseModel>>>(okResult.Value);
        }

        [Fact]
        public async void Get_User_By_Id_When_Details_Are_Returned_Test()
        {
            //Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            UserController userController = new UserController(userService.Object, mapper.Object);

            userService.Setup(x => x.GetUserProfile(It.IsAny<Guid>())).ReturnsAsync(new UserBo());
            mapper.Setup(x => x.Map<UserResponseModel>(It.IsAny<UserBo>())).Returns(new UserResponseModel());

            //Act
            var response = await userController.GetById(Guid.NewGuid());

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            Assert.IsType<GenericSuccessResponse<UserResponseModel>>(okResult.Value);
        }

        [Fact]
        public async void Get_User_Profile_Returns_User_Details_Test()
        {
            //Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            UserController userController = new UserController(userService.Object, mapper.Object);

            userService.Setup(x => x.GetUserProfile(It.IsAny<Guid>())).ReturnsAsync(new UserBo());
            mapper.Setup(x => x.Map<UserResponseModel>(It.IsAny<UserBo>())).Returns(new UserResponseModel());

            //Act
            var response = await userController.GetProfileAsync();

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            Assert.IsType<GenericSuccessResponse<UserResponseModel>>(okResult.Value);
        }

        [Fact]
        public async void Add_User_Returns_Success_Response_Test()
        {
            //Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            Mock<IMapper> mapper = new Mock<IMapper>();

            var userController = new UserController(userService.Object, mapper.Object);
            var userRequestModel = new UserRequestModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                Age = 20,
                City = "Test",
                Password = "Test"
            };

            var userBo = new UserBo()
            {
                UserId = Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"),
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                Age = 20,
                City = "Test",
                Password = "Test"
            };

            var userResponseModel = new UserResponseModel()
            {
                UserId = Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"),
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                Age = 20,
                City = "Test"
            };

            userService.Setup(x => x.AddUserAsync(It.IsAny<UserBo>())).ReturnsAsync(userBo);
            mapper.Setup(x => x.Map<UserBo>(It.IsAny<UserRequestModel>())).Returns(userBo);
            mapper.Setup(x => x.Map<UserResponseModel>(It.IsAny<UserBo>())).Returns(userResponseModel);

            //Act
            var response = await userController.AddAsync(userRequestModel);

            //Assert
            Assert.NotNull(response);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(response);

            Assert.Equal(nameof(userController.GetById), createdAtAction.ActionName);

            var routeValues = createdAtAction.RouteValues;

            Assert.True(routeValues?.ContainsKey("id"));

            Assert.Equal(Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"), routeValues["id"]);

            var successResponse = Assert.IsType<GenericSuccessResponse<UserResponseModel>>(createdAtAction.Value);

            Assert.Equal(201, successResponse.StatusCode);
            Assert.NotNull(successResponse.ResponseData);
            Assert.Equal(Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"), successResponse.ResponseData.UserId);
            Assert.Equal("Test", successResponse.ResponseData.FirstName);
        }

        [Fact]
        public async void Login_Successful_Login_Returns_Token()
        {
            //Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            Mock<IMapper> mapper = new Mock<IMapper>();

            var userController = new UserController(userService.Object, mapper.Object);

            var userCredentialsModel = new UserCredentialsModel()
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var userCredentialsBo = new UserCredentialsBo()
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            string token = "test-token";

            userService.Setup(x => x.AuthenticateUser(It.IsAny<UserCredentialsBo>())).ReturnsAsync(token);
            mapper.Setup(x => x.Map<UserCredentialsBo>(It.IsAny<UserCredentialsModel>())).Returns(userCredentialsBo);

            //Act
            var response = await userController.Login(userCredentialsModel);

            //Assert
            Assert.NotNull(response);

            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<string>>(okObjectResult.Value);

            Assert.Equal(200, successResponse.StatusCode);
            Assert.NotNull(successResponse.ResponseData);
            Assert.Equal(successResponse.ResponseMessage, ResponseMessage.Success.ToString());
        }
    }
}
