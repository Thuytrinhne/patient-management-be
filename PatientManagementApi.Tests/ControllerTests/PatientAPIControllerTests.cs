using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientManagementApi.Controllers;
using PatientManagementApi.Core.Pagination;
using PatientManagementApi.Dtos;
using PatientManagementApi.Dtos.ContactInfor;
using PatientManagementApi.Dtos.Patient;
using PatientManagementApi.Enums;
using PatientManagementApi.Models;
using PatientManagementApi.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagementApi.Tests.ControllerTests
{
    public class PatientAPIControllerTests
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PatientAPIController _controller;
        private readonly ResponseDto _response;

        public PatientAPIControllerTests()
        {
            _mockPatientService = new Mock<IPatientService>();
            _mockMapper = new Mock<IMapper>();
            _response = new ResponseDto();
            _controller = new PatientAPIController(_mockMapper.Object, _mockPatientService.Object);
        }
        #region Get 
        [Fact]
        public async Task Get_WhenPatientsExist_ReturnsOkResult()
        {
            // Arrange
            var request = new PaginationRequest();
            var patients = new PaginationResult<Patient>();
            var patientDtos = new PaginationResult<GetPatientsResponseDto>();

            _mockPatientService.Setup(s => s.GetAllPatientAsync(request, null, null, null, null, null, null, null))
                               .ReturnsAsync(patients);
            _mockMapper.Setup(m => m.Map<PaginationResult<GetPatientsResponseDto>>(patients))
                       .Returns(patientDtos);

            // Act
            var result = await _controller.Get(request, null, null, null, null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(patientDtos, ((ResponseDto)okResult.Value).Result);
        }

        [Fact]
        public async Task Get_WhenNoPatients_ReturnsNotFound()
        {
            // Arrange
            var request = new PaginationRequest();
            _mockPatientService.Setup(s => s.GetAllPatientAsync(request, null, null, null, null, null, null, null))
                               .ReturnsAsync((PaginationResult<Patient>)null);

            // Act
            var result = await _controller.Get(request, null, null, null, null, null, null, null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        #endregion
        #region GetPatientById
        [Fact]
        public async Task GetPatientById_WhenPatientExists_ReturnsOkResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patient = new Patient();
            var patientDto = new GetPatientsResponseDto();

            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<GetPatientsResponseDto>(patient)).Returns(patientDto);

            // Act
            var result = await _controller.Get(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(patientDto, ((ResponseDto)okResult.Value).Result);
        }

        [Fact]
        public async Task GetPatientById_WhenPatientDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync((Patient)null);

            // Act
            var result = await _controller.Get(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        #endregion
        #region Post
        [Fact]
        public async Task Post_WhenPatientCreated_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var request = new CreatePatientRequestDto();
            var patient = new Patient();
            var newPatientId = Guid.NewGuid();

            _mockMapper.Setup(m => m.Map<Patient>(request)).Returns(patient);
            _mockPatientService.Setup(s => s.AddPatientAsync(patient)).ReturnsAsync(newPatientId);

            // Act
            var result = await _controller.Post(request);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal(201, createdAtRouteResult.StatusCode);
            Assert.Equal(newPatientId, ((ResponseDto)createdAtRouteResult.Value).Result);
        }
        #endregion
        #region Patch
        [Fact]
        public async Task Patch_WhenPatientUpdated_ReturnsOkResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var request = new UpdatePatientRequestDto();
            var patient = new Patient { Id = patientId };

            _mockMapper.Setup(m => m.Map<Patient>(request)).Returns(patient);
            _mockPatientService.Setup(s => s.UpdatePatientAsync(patient)).ReturnsAsync(patientId);

            // Act
            var result = await _controller.Patch(patientId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(patientId, ((ResponseDto)okResult.Value).Result);
        }
        #endregion
        #region Delete
        [Fact] 
        public async Task Delete_WhenPatientDeleted_ReturnsOkResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            _mockPatientService.Setup(s => s.DeletePatientAsync(patientId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region GetAddressByPatientId
        [Fact] 
        public async Task GetAddressByPatientId_WhenPatientExistsAndHasAddresses_ReturnsOkResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patient = new Patient { Addresses = new List<Address> { new Address() } };
            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<IEnumerable<GetAddressDto>>(patient.Addresses)).Returns(new List<GetAddressDto>());

            // Act
            var result = await _controller.GetAddressByPatientId(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAddressByPatientId_WhenPatientDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync((Patient)null);

            // Act
            var result = await _controller.GetAddressByPatientId(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetAddressByPatientId_WhenPatientExistsButNoAddresses_ReturnsNotFound()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patient = new Patient { Addresses = new List<Address>() };
            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync(patient);

            // Act
            var result = await _controller.GetAddressByPatientId(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        #endregion

        #region GetContactInforsByPatientId
        [Fact] 
        public async Task GetContactInforsByPatientId_WhenPatientExistsAndHasContactInfors_ReturnsOkResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patient = new Patient { ContactInfors = new List<ContactInfor> { new ContactInfor() } };
            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<IEnumerable<GetContactInforDto>>(patient.ContactInfors)).Returns(new List<GetContactInforDto>());

            // Act
            var result = await _controller.GetContactInforsByPatientId(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetContactInforsByPatientId_WhenPatientDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync((Patient)null);

            // Act
            var result = await _controller.GetContactInforsByPatientId(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetContactInforsByPatientId_WhenPatientExistsButNoContactInfors_ReturnsNotFound()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patient = new Patient { ContactInfors = new List<ContactInfor>() };
            _mockPatientService.Setup(s => s.GetPatientById(patientId)).ReturnsAsync(patient);

            // Act
            var result = await _controller.GetContactInforsByPatientId(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        #endregion

        #region Deactivat
        [Fact]
        public async Task Deactivate_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var deactiveReason = "No longer active";
            _mockPatientService.Setup(s => s.DeactivePatient(patientId, deactiveReason)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Deactivate(patientId, deactiveReason);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

    }
}
