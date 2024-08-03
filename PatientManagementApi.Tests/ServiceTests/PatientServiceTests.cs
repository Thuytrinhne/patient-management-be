using Moq;
using PatientManagementApi.Services.IServices;
using PatientManagementApi.Services;
using PatientManagementApi.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientManagementApi.Models;
using PatientManagementApi.Extensions.Exceptions;
using PatientManagementApi.Core.Pagination;

namespace PatientManagementApi.Tests.ServiceTests
{
    public class PatientServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCacheService = new Mock<ICacheService>();
            _service = new PatientService(_mockUnitOfWork.Object, _mockCacheService.Object);
        }
        [Fact]
        public async Task AddPatientAsync_ShouldReturnPatientId()
        {
            // Arrange
            var patient = new Patient { Id = Guid.NewGuid() };
            _mockUnitOfWork.Setup(x => x.Patients.AddAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.AddPatientAsync(patient);

            // Assert
            Assert.Equal(patient.Id, result);
        }
        [Fact]
        public async Task DeactivePatient_WhenPatientNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Patients.GetById(It.IsAny<Guid>())).Returns((Patient)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeactivePatient(Guid.NewGuid(), "No longer needed"));
        }

        [Fact]
        public async Task DeactivePatient_WhenPatientAlreadyDeactivated_ShouldThrowBadRequestException()
        {
            // Arrange
            var patient = new Patient { IsActive = false };
            _mockUnitOfWork.Setup(x => x.Patients.GetById(It.IsAny<Guid>())).Returns(patient);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.DeactivePatient(Guid.NewGuid(), "Already deactivated"));
        }
        [Fact]
        public async Task DeletePatientAsync_WhenPatientNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Patients.GetById(It.IsAny<Guid>())).Returns((Patient)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeletePatientAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task DeletePatientAsync_WhenPatientFound_ShouldDeletePatient()
        {
            // Arrange
            var patient = new Patient { Id = Guid.NewGuid() };
            _mockUnitOfWork.Setup(x => x.Patients.GetById(It.IsAny<Guid>())).Returns(patient);
            _mockUnitOfWork.Setup(x => x.Patients.Delete(It.IsAny<Patient>())).Verifiable();
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _mockCacheService.Setup(x => x.DeletePatient(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeletePatientAsync(patient.Id);

            // Assert
            _mockUnitOfWork.Verify(x => x.Patients.Delete(patient), Times.Once);
        }
        [Fact]
        public async Task UpdatePatientAsync_WhenPatientNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Patients.GetById(It.IsAny<Guid>())).Returns((Patient)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdatePatientAsync(new Patient { Id = Guid.NewGuid() }));
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientFound_ShouldUpdateFields()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patient = new Patient { Id = patientId, FirstName = "Old Name" };
            _mockUnitOfWork.Setup(x => x.Patients.GetById(patientId)).Returns(patient);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.UpdatePatientAsync(new Patient { Id = patientId, FirstName = "New Name" });

            // Assert
            Assert.Equal(patientId, result);
            Assert.Equal("New Name", patient.FirstName);
        }
        [Fact]
        public async Task GetAllPatientAsync_ShouldReturnPatients()
        {
            var request = new PaginationRequest();
            var patients = new PaginationResult<Patient>();
            _mockUnitOfWork.Setup(x => x.Patients.SearchPatientAsync(request, null, null, null, null, null, null, null)).ReturnsAsync(patients);

            var result = await _service.GetAllPatientAsync(request, null, null, null, null, null, null, null);

            Assert.Equal(patients, result);
        }
        [Fact]
        public async Task GetPatientById_WhenInCache_ShouldReturnFromCache()
        {
            var patient = new Patient { Id = Guid.NewGuid() };
            _mockCacheService.Setup(x => x.GetPatient(patient.Id)).ReturnsAsync(patient);

            var result = await _service.GetPatientById(patient.Id);

            Assert.Equal(patient, result);
        }

        [Fact]
        public async Task GetPatientById_WhenNotInCache_ShouldReturnFromDbAndStoreInCache()
        {
            var patient = new Patient { Id = Guid.NewGuid() };
            _mockCacheService.Setup(x => x.GetPatient(patient.Id)).ReturnsAsync((Patient)null);
            _mockUnitOfWork.Setup(x => x.Patients.GetById(patient.Id)).Returns(patient);
            _mockCacheService.Setup(x => x.StorePatient(patient)).Returns(Task.CompletedTask);

            var result = await _service.GetPatientById(patient.Id);

            Assert.Equal(patient, result);
            _mockCacheService.Verify(x => x.StorePatient(patient), Times.Once);
        }
    }
}
