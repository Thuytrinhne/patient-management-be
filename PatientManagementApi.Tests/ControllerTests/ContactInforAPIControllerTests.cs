using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientManagementApi.Controllers;
using PatientManagementApi.Dtos;
using PatientManagementApi.Dtos.ContactInfor;
using PatientManagementApi.Models;
using PatientManagementApi.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagementApi.Tests.ControllerTests
{
    public class ContactInforAPIControllerTests
    {
        private readonly Mock<IContactInforService> _mockContactInforService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ContactInforAPIController _controller;
        private readonly ResponseDto _response;

        public ContactInforAPIControllerTests()
        {
            _mockContactInforService = new Mock<IContactInforService>();
            _mockMapper = new Mock<IMapper>();
            _response = new ResponseDto();
            _controller = new ContactInforAPIController(_mockMapper.Object, _mockContactInforService.Object);
        }
 
        #region GetContactInforById
        [Fact]
        public async Task GetContactInforById_WhenContactInforDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var contactInforId = Guid.NewGuid();
            _mockContactInforService.Setup(x => x.GetContactInforById(contactInforId)).Returns((ContactInfor)null);

            // Act
            var result = await _controller.Get(contactInforId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.False(((ResponseDto)notFoundResult.Value).IsSuccess);
        }

        [Fact]
        public async Task GetContactInforById_WhenContactInforExists_ReturnsOk()
        {
            // Arrange
            var contactInforId = Guid.NewGuid();
            var contactInfor = new ContactInfor();
            var contactInforDto = new GetContactInforDto();
            _mockContactInforService.Setup(x => x.GetContactInforById(contactInforId)).Returns(contactInfor);
            _mockMapper.Setup(m => m.Map<GetContactInforDto>(contactInfor)).Returns(contactInforDto);

            // Act
            var result = await _controller.Get(contactInforId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(contactInforDto, ((ResponseDto)okResult.Value).Result);
        }
        #endregion
        #region Post
        [Fact]
        public async Task Post_WhenContactInforCreated_ReturnsCreatedAtRoute()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var request = new UpsertContactInforDto();
            var contactInfor = new ContactInfor { PatientId = patientId };
            var newContactInforId = Guid.NewGuid();
            _mockMapper.Setup(m => m.Map<ContactInfor>(request)).Returns(contactInfor);
            _mockContactInforService.Setup(x => x.AddContactInforAsync(contactInfor)).ReturnsAsync(newContactInforId);

            // Act
            var result = await _controller.Post(patientId, request);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal(newContactInforId, ((ResponseDto)createdAtRouteResult.Value).Result);
        }
        #endregion
        #region Patch
        [Fact]
        public async Task Patch_WhenContactInforUpdated_ReturnsOk()
        {
            // Arrange
            var contactInforId = Guid.NewGuid();
            var request = new UpsertContactInforDto();
            var contactInfor = new ContactInfor { Id = contactInforId };
            _mockMapper.Setup(m => m.Map<ContactInfor>(request)).Returns(contactInfor);
            _mockContactInforService.Setup(x => x.UpdateContactInforAsync(contactInfor)).ReturnsAsync(contactInforId);

            // Act
            var result = await _controller.Patch(contactInforId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(contactInforId, ((ResponseDto)okResult.Value).Result);
        }
        #endregion
        #region Delete
        [Fact]
        public async Task Delete_WhenContactInforDeleted_ReturnsOk()
        {
            // Arrange
            var contactInforId = Guid.NewGuid();
            _mockContactInforService.Setup(x => x.DeleteContactInforAsync(contactInforId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(contactInforId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(((ResponseDto)okResult.Value).IsSuccess);
        }
        #endregion
    }
}
