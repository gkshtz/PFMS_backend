﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PFMS.API.Controllers;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Tests.Controllers
{
    public class UserControllerSetup
    {
        public readonly Mock<IUserService> userService;
        public readonly Mock<IOneTimePasswordsService> otpService;
        public readonly Mock<IMapper> mapper;
        public readonly UserController userController;

        public UserControllerSetup()
        {
            userService = new Mock<IUserService>();
            otpService = new Mock<IOneTimePasswordsService>();
            mapper = new Mock<IMapper>();
            userController = new UserController(userService.Object, mapper.Object, otpService.Object);
        }
    }

    public class UserControllerTests: IClassFixture<UserControllerSetup>
    {
        private readonly UserControllerSetup _setup;
        public UserControllerTests(UserControllerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public async void Gell_All_Users_When_Users_List_Is_Returned_Test()
        {
            // Arrange
            _setup.userService.Setup(x => x.GetAllUsers()).ReturnsAsync(new List<UserBo>());
            _setup.mapper.Setup(x => x.Map<List<UserResponseModel>>(It.IsAny<List<UserBo>>())).Returns(new List<UserResponseModel>());

            //Act
            var response = await _setup.userController.GetAllAsync();

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
            _setup.userService.Setup(x => x.GetUserProfile(It.IsAny<Guid>())).ReturnsAsync(new UserBo());
            _setup.mapper.Setup(x => x.Map<UserResponseModel>(It.IsAny<UserBo>())).Returns(new UserResponseModel());

            //Act
            var response = await _setup.userController.GetById(Guid.NewGuid());

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
            _setup.userService.Setup(x => x.GetUserProfile(It.IsAny<Guid>())).ReturnsAsync(new UserBo());
            _setup.mapper.Setup(x => x.Map<UserResponseModel>(It.IsAny<UserBo>())).Returns(new UserResponseModel());

            //Act
            var response = await _setup.userController.GetProfileAsync();

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
                Id = Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"),
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                Age = 20,
                City = "Test",
                Password = "Test"
            };

            var userResponseModel = new UserResponseModel()
            {
                Id = Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"),
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                Age = 20,
                City = "Test"
            };

            _setup.userService.Setup(x => x.AddUserAsync(It.IsAny<UserBo>())).ReturnsAsync(userBo);
            _setup.mapper.Setup(x => x.Map<UserBo>(It.IsAny<UserRequestModel>())).Returns(userBo);
            _setup.mapper.Setup(x => x.Map<UserResponseModel>(It.IsAny<UserBo>())).Returns(userResponseModel);

            //Act
            var response = await _setup.userController.AddAsync(userRequestModel);

            //Assert
            Assert.NotNull(response);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(response);

            Assert.Equal(nameof(_setup.userController.GetById), createdAtAction.ActionName);

            var routeValues = createdAtAction.RouteValues;

            Assert.True(routeValues?.ContainsKey("id"));

            Assert.Equal(Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"), routeValues["id"]);

            var successResponse = Assert.IsType<GenericSuccessResponse<UserResponseModel>>(createdAtAction.Value);

            Assert.Equal(201, successResponse.StatusCode);
            Assert.NotNull(successResponse.ResponseData);
            Assert.Equal(Guid.Parse("8871e8f9-ddb1-4061-89b6-b81846dd7b6b"), successResponse.ResponseData.Id);
            Assert.Equal("Test", successResponse.ResponseData.FirstName);
        }

        [Fact]
        public async void Login_Successful_Login_Returns_Token()
        {
            //Arrange
            Mock<HttpContext> context = new Mock<HttpContext>();
            Mock<HttpResponse> httpResponse = new Mock<HttpResponse>();
            Mock<IResponseCookies> cookies = new Mock<IResponseCookies>();

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

            var tokenBo = new TokenBo()
            {
                AccessToken = "testToken",
                RefreshToken = "testToken"
            };

            _setup.userService.Setup(x => x.AuthenticateUser(It.IsAny<UserCredentialsBo>())).ReturnsAsync(tokenBo);
            _setup.mapper.Setup(x => x.Map<UserCredentialsBo>(It.IsAny<UserCredentialsModel>())).Returns(userCredentialsBo);

            context.Setup(x => x.Response).Returns(httpResponse.Object);
            httpResponse.Setup(x => x.Cookies).Returns(cookies.Object);

            _setup.userController.ControllerContext = new ControllerContext()
            {
                HttpContext = context.Object
            };

            //Act
            var response = await _setup.userController.Login(userCredentialsModel);

            //Assert
            Assert.NotNull(response);

            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<string>>(okObjectResult.Value);

            Assert.Equal(200, successResponse.StatusCode);
            Assert.NotNull(successResponse.ResponseData);
            Assert.IsType<string>(successResponse.ResponseData);
            Assert.Equal(successResponse.ResponseMessage, ResponseMessage.Success.ToString());
        }

        [Fact]
        public async void Update_User_Profile_Successful_Update_Test()
        {
            // Arrange
            var userModel = new UserUpdateRequestModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 12,
                City = "Test",
                Email = "test@gmail.com"
            };

            var userBo = new UserBo()
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 12,
                City = "Test",
                Email = "test@gmail.com"
            };

            _setup.userService.Setup(x => x.UpdateUserProfile(It.IsAny<UserBo>(), It.IsAny<Guid>()));

            //Act
            var response = await _setup.userController.PatchAsync(userModel);

            //Assert
            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.NotNull(successResponse);

            Assert.Equal(200, successResponse.StatusCode);
            Assert.True(successResponse.ResponseData);
            Assert.Equal(ResponseMessage.Success.ToString(), successResponse.ResponseMessage);
        }

        [Fact]
        public async void Update_Password_Updates_Successfully_Test()
        {
            //Arrange
            var passwordUpdateModel = new PasswordUpdateRequestModel()
            {
                NewPassword = "testNewPassword",
                OldPassword = "testOldPassword"
            };

            _setup.userService.Setup(x => x.UpdatePassword(It.IsAny<string>(), It.IsAny<string>(), Guid.NewGuid()));

            //Act
            var response = await _setup.userController.UpdatePassword(passwordUpdateModel);

            //Assert
            _setup.userService.Verify(x => x.UpdatePassword(passwordUpdateModel.OldPassword, passwordUpdateModel.NewPassword, It.IsAny<Guid>()), Times.Once);

            Assert.NotNull(response);

            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.Equal(200, successResponse.StatusCode);
            Assert.True(successResponse.ResponseData);
        }

        [Fact]
        public async void Refresh_Access_Token_Refreshes_Successfully_Test()
        {
            //Arrange
            string testRefreshAccessToken = "Test Refresh Access Token";

            _setup.userService.Setup(x => x.RefreshAccessToken()).ReturnsAsync(testRefreshAccessToken);

            //Act
            var response = await _setup.userController.GetRefreshedAccessToken();

            //Assert
            Assert.NotNull(response);
            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<string>>(okResult.Value);

            Assert.Equal(200, successResponse.StatusCode);
            Assert.Equal(testRefreshAccessToken, successResponse.ResponseData);
        }

        [Fact]
        public async void Send_Otp_Successfuly_Sent_Test()
        {
            //Arrange
            Mock<HttpContext> httpContext = new Mock<HttpContext>();
            Mock<HttpResponse> httpResponse = new Mock<HttpResponse>();
            Mock<IResponseCookies> cookies = new Mock<IResponseCookies>();

            var sendOtpModel = new SendOtpRequestModel()
            {
                EmailAddress = "test@gmail.com"
            };
            var uniqueDeviceId = Guid.Parse("24d7f0b1-a608-4de2-a6a8-efe49622a4d8");

            _setup.otpService.Setup(x => x.GenerateAndSendOtp(It.IsAny<string>())).ReturnsAsync(uniqueDeviceId);

            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);
            httpResponse.Setup(x=>x.Cookies).Returns(cookies.Object);

            _setup.userController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };

            //Act
            var response = await _setup.userController.SendOtpAsync(sendOtpModel);

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);
            Assert.True(successResponse.ResponseData);
            Assert.Equal(200, successResponse.StatusCode);
        }

        [Fact]
        public async void Verify_OTP_Successfuly_Verified()
        {
            //Arrange
            _setup.otpService.Setup(x => x.VerifyOtp(It.IsAny<string>(), It.IsAny<string>()));

            var verifyOtpModel = new VerifyOtpRequestModel()
            {
                EmailAddress = "test@gmail.com",
                Otp = "123456"
            };

            //Act
            var response = await _setup.userController.VerifyOtpAsync(verifyOtpModel);

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);

            Assert.True(successResponse.ResponseData);
            Assert.Equal(200, successResponse.StatusCode);
            Assert.Equal(ResponseMessage.Success.ToString(), successResponse.ResponseMessage);
        }

        [Fact]
        public async void Reset_Password_Successfull_Reset()
        {
            //Arrange
            var resetPasswordModel = new ResetPasswordRequestModel()
            {
                NewPassword = "testPassword"
            };
            
            //Act
            var response = await _setup.userController.ResetPasswordAsync(resetPasswordModel);

            //Assert
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);

            Assert.True(successResponse.ResponseData);
            Assert.Equal(200, successResponse.StatusCode);
            Assert.Equal(ResponseMessage.Success.ToString(), successResponse.ResponseMessage);
        }

        [Fact]
        public async void Delete_User_Successful_Delete_Test()
        {
            //Arrange
            Guid userId = Guid.Parse("0ffa99df-59ac-4356-9241-069ef3c765bb");

            _setup.userService.Setup(x => x.DeleteUserAsync(It.IsAny<Guid>()));

            //Act
            var response = await _setup.userController.DeleteAsync(userId);

            //Arrange
            Assert.NotNull(response);

            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);

            var successResponse = Assert.IsType<GenericSuccessResponse<bool>>(okResult.Value);

            Assert.Equal(200, successResponse.StatusCode);
            Assert.True(successResponse.ResponseData);
            Assert.Equal(ResponseMessage.Success.ToString(), successResponse.ResponseMessage);
        }
    }
}
