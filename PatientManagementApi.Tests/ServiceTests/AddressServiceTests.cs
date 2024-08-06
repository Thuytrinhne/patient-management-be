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
    public class AddressServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICacheService> _mockCacheService;

        private readonly AddressService _addressService;

        public AddressServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCacheService = new Mock<ICacheService>();
            _addressService = new AddressService(_mockCacheService.Object, _mockUnitOfWork.Object);
        }
        [Fact]
        public async Task AddAddressAsync_WhenPatientNotFound_ShouldThrowNotFoundException()
        {
            _mockUnitOfWork.Setup(x => x.Patients.GetById(It.IsAny<Guid>())).Returns((Patient)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _addressService.AddAddressAsync(new Address { PatientId = Guid.NewGuid() }));
        }

        [Fact]
        public async Task AddAddressAsync_WhenPatientHasLessThanTwoAddresses_ShouldAddAddress()
        {
            var patient = new Patient { Id = Guid.NewGuid(), Addresses = new List<Address>() };
            _mockUnitOfWork.Setup(x => x.Patients.GetById(patient.Id)).Returns(patient);
            _mockUnitOfWork.Setup(x => x.Addresses.AddAsync(It.IsAny<Address>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var address = new Address { PatientId = patient.Id };
            var result = await _addressService.AddAddressAsync(address);

            Assert.Equal(address.Id, result);
        }
        [Fact]
        public async Task DeleteAddressAsync_WhenAddressNotFound_ShouldThrowNotFoundException()
        {
            _mockUnitOfWork.Setup(x => x.Addresses.GetById(It.IsAny<Guid>())).Returns((Address)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _addressService.DeleteAddressAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task DeleteAddressAsync_WhenAddressIsDefaultAndNoOtherAddresses_ShouldThrowBadRequestException()
        {
            var address = new Address { Id = Guid.NewGuid(), IsDefault = true, PatientId = Guid.NewGuid() };
            _mockUnitOfWork.Setup(x => x.Addresses.GetById(address.Id)).Returns(address);
            _mockUnitOfWork.Setup(x => x.Patients.GetById(address.PatientId)).Returns(new Patient { Addresses = new List<Address> { address } });

            await Assert.ThrowsAsync<BadRequestException>(() => _addressService.DeleteAddressAsync(address.Id));
        }
        [Fact]
        public async Task UpdateAddressAsync_WhenAddressNotFound_ShouldThrowNotFoundException()
        {
            _mockUnitOfWork.Setup(x => x.Addresses.GetById(It.IsAny<Guid>())).Returns((Address)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _addressService.UpdateAddressAsync(new Address { Id = Guid.NewGuid() }));
        }

        [Fact]
        public async Task UpdateAddressAsync_WhenAddressExists_ShouldUpdateFields()
        {
            var address = new Address { Id = Guid.NewGuid(), Province = "OldProvince" };
            _mockUnitOfWork.Setup(x => x.Addresses.GetById(address.Id)).Returns(address);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var updatedAddress = new Address { Id = address.Id, Province = "NewProvince" };
            var result = await _addressService.UpdateAddressAsync(updatedAddress);

            Assert.Equal(address.Id, result);
            Assert.Equal("NewProvince", address.Province);
        }
        [Fact]
        public async Task GetAllAddressAsync_WhenPatientIdProvided_ShouldReturnFilteredAddresses()
        {
            var patientId = Guid.NewGuid();
            var addresses = new List<Address> { new Address { PatientId = patientId } };
            _mockUnitOfWork.Setup(x => x.Addresses.GetAllAsync(It.IsAny<Expression<Func<Address, bool>>>())).ReturnsAsync(addresses);

            var result = await _addressService.GetAllAddressAsync(patientId);

            Assert.Single(result);
            Assert.Equal(patientId, result.First().PatientId);
        }


    }
}
