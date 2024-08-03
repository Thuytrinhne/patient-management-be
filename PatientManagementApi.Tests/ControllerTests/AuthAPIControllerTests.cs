using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientManagementApi.Controllers;
using PatientManagementApi.Dtos;
using PatientManagementApi.Dtos.Auth;
using PatientManagementApi.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagementApi.Tests.ControllerTests
{
    public class AuthAPIControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthAPIController _controller;
        private readonly ResponseDto _response;

        public AuthAPIControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _response = new ResponseDto();
            _controller = new AuthAPIController(_mockAuthService.Object);
        }
        [Fact]
        public async Task Login_WhenCredentialsAreValid_ReturnsOk()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password123" };
            var expectedResponse = new LoginResponseDto { AccessToken = "someToken", UserId = Guid.NewGuid() };

            _mockAuthService.Setup(x => x.Login(loginDto)).ReturnsAsync(expectedResponse);
            _response.Result = expectedResponse;

            // Act
            var result = await _controller.Login(loginDto) as OkObjectResult;
            var actualResponse = result.Value as ResponseDto;

            // Assert
            Assert.Equal(expectedResponse.AccessToken, ((LoginResponseDto)actualResponse.Result).AccessToken);
            Assert.Equal(expectedResponse.UserId, ((LoginResponseDto)actualResponse.Result).UserId);
        }

    }
}
