using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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
        public async void Test()
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
    }
}
