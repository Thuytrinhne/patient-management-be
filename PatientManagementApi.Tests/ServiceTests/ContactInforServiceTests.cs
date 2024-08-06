using Moq;
using PatientManagementApi.Extensions.Exceptions;
using PatientManagementApi.Models;
using PatientManagementApi.Services;
using PatientManagementApi.Services.IServices;
using PatientManagementApi.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagementApi.Tests.ServiceTests
{
    public class ContactInforServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ContactInforService _contactInforService;
        private readonly Mock<ICacheService> _mockCacheService;


        public ContactInforServiceTests()
        {
            _mockCacheService = new Mock<ICacheService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _contactInforService = new ContactInforService(_mockCacheService.Object, _mockUnitOfWork.Object);
        }
        [Fact]
        public async Task AddContactInforAsync_WhenPatientNotFound_ShouldThrowNotFoundException()
        {
            _mockUnitOfWork.Setup(x => x.Patients.GetById(It.IsAny<Guid>())).Returns((Patient)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _contactInforService.AddContactInforAsync(new ContactInfor { PatientId = Guid.NewGuid() }));
        }

        [Fact]
        public async Task AddContactInforAsync_WhenPatientExists_ShouldAddContactInfor()
        {
            var patient = new Patient { Id = Guid.NewGuid() };
            var contactInfor = new ContactInfor { Id = Guid.NewGuid(), PatientId = patient.Id };
            _mockUnitOfWork.Setup(x => x.Patients.GetById(patient.Id)).Returns(patient);
            _mockUnitOfWork.Setup(x => x.ContactInfors.AddAsync(contactInfor)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _contactInforService.AddContactInforAsync(contactInfor);

            Assert.Equal(contactInfor.Id, result);
        }
        [Fact]
        public async Task DeleteContactInforAsync_WhenContactInforNotFound_ShouldThrowNotFoundException()
        {
            _mockUnitOfWork.Setup(x => x.ContactInfors.GetById(It.IsAny<Guid>())).Returns((ContactInfor)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _contactInforService.DeleteContactInforAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task DeleteContactInforAsync_WhenContactInforExists_ShouldDeleteContactInfor()
        {
            var contactInfor = new ContactInfor { Id = Guid.NewGuid() };
            _mockUnitOfWork.Setup(x => x.ContactInfors.GetById(contactInfor.Id)).Returns(contactInfor);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            await _contactInforService.DeleteContactInforAsync(contactInfor.Id);

            _mockUnitOfWork.Verify(x => x.ContactInfors.Delete(contactInfor), Times.Once);
        }
        [Fact]
        public async Task UpdateContactInforAsync_WhenContactInforNotFound_ShouldThrowNotFoundException()
        {
            _mockUnitOfWork.Setup(x => x.ContactInfors.GetById(It.IsAny<Guid>())).Returns((ContactInfor)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _contactInforService.UpdateContactInforAsync(new ContactInfor { Id = Guid.NewGuid() }));
        }

        [Fact]
        public async Task UpdateContactInforAsync_WhenContactInforExists_ShouldUpdateFields()
        {
            var contactInfor = new ContactInfor { Id = Guid.NewGuid(), Value = "Old Value" };
            _mockUnitOfWork.Setup(x => x.ContactInfors.GetById(contactInfor.Id)).Returns(contactInfor);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var updatedContactInfor = new ContactInfor { Id = contactInfor.Id, Value = "New Value" };
            var result = await _contactInforService.UpdateContactInforAsync(updatedContactInfor);

            Assert.Equal(contactInfor.Id, result);
            Assert.Equal("New Value", contactInfor.Value);
        }
        [Fact]
        public async Task GetAllContactInforAsync_WhenPatientIdProvided_ShouldReturnFilteredContactInfors()
        {
            var patientId = Guid.NewGuid();
            var contactInfors = new List<ContactInfor> { new ContactInfor { PatientId = patientId } };
            _mockUnitOfWork.Setup(x => x.ContactInfors.GetAllAsync(It.IsAny<Expression<Func<ContactInfor, bool>>>())).ReturnsAsync(contactInfors);

            var result = await _contactInforService.GetAllContactInforAsync(patientId);

            Assert.Single(result);
            Assert.Equal(patientId, result.First().PatientId);
        }

    }
}
