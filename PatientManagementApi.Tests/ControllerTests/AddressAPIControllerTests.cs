using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientManagementApi.Controllers;
using PatientManagementApi.Dtos;
using PatientManagementApi.Dtos.Address;
using PatientManagementApi.Models;
using PatientManagementApi.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagementApi.Tests.ControllerTests
{
    public class AddressAPIControllerTests
    {
        private readonly Mock<IAddressService> _mockAddressService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddressAPIController _controller;
        private readonly ResponseDto _response;

        public AddressAPIControllerTests()
        {
            _mockAddressService = new Mock<IAddressService>();
            _mockMapper = new Mock<IMapper>();
            _response = new ResponseDto();
            _controller = new AddressAPIController(_mockMapper.Object, _mockAddressService.Object);
        }
     
        #region GetAddressById
        [Fact]

        public async Task GetAddressById_WhenAddressDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            _mockAddressService.Setup(x => x.GetAddressById(addressId)).Returns((Address)null);

            // Act
            var result = await _controller.Get(addressId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.False(((ResponseDto)notFoundResult.Value).IsSuccess);
        }

        [Fact]
        public async Task GetAddressById_WhenAddressExists_ReturnsOk()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var address = new Address();
            var addressDto = new GetAddressDto();
            _mockAddressService.Setup(x => x.GetAddressById(addressId)).Returns(address);
            _mockMapper.Setup(m => m.Map<GetAddressDto>(address)).Returns(addressDto);

            // Act
            var result = await _controller.Get(addressId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(addressDto, ((ResponseDto)okResult.Value).Result);
        }
        #endregion
        #region Post
        [Fact] 
        public async Task Post_WhenAddressCreated_ReturnsCreatedAtRoute()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var request = new UpsertAddressDto();
            var address = new Address { PatientId = patientId };
            var newAddressId = Guid.NewGuid();
            _mockMapper.Setup(m => m.Map<Address>(request)).Returns(address);
            _mockAddressService.Setup(x => x.AddAddressAsync(address)).ReturnsAsync(newAddressId);

            // Act
            var result = await _controller.Post(patientId, request);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal(newAddressId, ((ResponseDto)createdAtRouteResult.Value).Result);
        }
        #endregion
        #region Patch
        [Fact] 
        public async Task Patch_WhenAddressUpdated_ReturnsOk()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var request = new UpsertAddressDto();
            var address = new Address { Id = addressId };
            _mockMapper.Setup(m => m.Map<Address>(request)).Returns(address);
            _mockAddressService.Setup(x => x.UpdateAddressAsync(address)).ReturnsAsync(addressId);

            // Act
            var result = await _controller.Patch(addressId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(addressId, ((ResponseDto)okResult.Value).Result);
        }
        #endregion
        #region
        [Fact]
        public async Task Delete_WhenAddressDeleted_ReturnsOk()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            _mockAddressService.Setup(x => x.DeleteAddressAsync(addressId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(addressId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(((ResponseDto)okResult.Value).IsSuccess);
        }
        #endregion

    }
}
