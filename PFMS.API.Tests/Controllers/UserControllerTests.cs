using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PFMS.API.Controllers;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;

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
    }
}
