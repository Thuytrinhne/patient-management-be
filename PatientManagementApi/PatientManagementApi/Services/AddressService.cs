namespace PatientManagementApi.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> AddAddressAsync(Address address)
        {
            Patient PatientFrmDb = _unitOfWork.Patients.GetById(address.PatientId);
            if (PatientFrmDb is null)
                throw new NotFoundException("A patient not found");
            
            if (PatientFrmDb.Addresses.Count <2)
            {
                await _unitOfWork.Addresses.AddAsync(address);
                if (address.IsDefault)
                {
                   await UpdateDefaultAddressAsync(address.PatientId, address.Id);
                }
                await _unitOfWork.SaveChangesAsync();
                return address.Id;

            }
            throw new BadRequestException("A patient cannot have more than two addresses.");

        }

        public async  Task DeleteAddressAsync(Guid id)
        {
            var addressFrmDb = _unitOfWork.Addresses.GetById(id);
            if (addressFrmDb is null)
                throw new NotFoundException("Address not found.");
            if(addressFrmDb.IsDefault)
            {
                if ( ! await IsRemainDefaultAddress(addressFrmDb.PatientId, id))
                    throw new BadRequestException("Cannot delete the default address as there are no other addresses available.");

            }
            _unitOfWork.Addresses.Delete(addressFrmDb);
            await _unitOfWork.SaveChangesAsync();
        }

        private async  Task<bool> IsRemainDefaultAddress(Guid PatientId, Guid addressid)
        {
            // Kiểm tra xem còn địa chỉ nào khác không
            var patientFrmDb = _unitOfWork.Patients.GetById(PatientId);
            var otherAddresses = patientFrmDb.Addresses.Where(a => a.Id != addressid).ToList();
            if (!otherAddresses.Any())
            {
                return false;
            }
            else
            {
                // Đặt địa chỉ đầu tiên trong danh sách còn lại làm mặc định
                otherAddresses.First().IsDefault = true;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public Address GetAddressById(Guid id)
        {
            return _unitOfWork.Addresses.GetById(id);

        }
        public async Task<IEnumerable<Address>> GetAllAddressAsync(Guid? PatientId)
        {
            if (PatientId.HasValue)
            {
                return await _unitOfWork.Addresses.GetAllAsync(a => a.PatientId == PatientId.Value);
            }
            else
            {
                return await _unitOfWork.Addresses.GetAllAsync();
            }
        }


        public async Task<Guid> UpdateAddressAsync(Address address)
        {
            var addressFrmDb = _unitOfWork.Addresses.GetById(address.Id);
            if (addressFrmDb is null)
            {
                throw new NotFoundException("Address not found.");
            }
            if (!address.IsDefault && address.IsDefault != addressFrmDb.IsDefault)
            {

                if (await IsRemainDefaultAddress(addressFrmDb.PatientId, addressFrmDb.Id))
                {
                    addressFrmDb.IsDefault = false;
                }
                else
                {
                    throw new BadRequestException("Cannot update to the undefault address as there are no other addresses available.");

                }
            }
            if (!String.IsNullOrEmpty(address.Province) && address.Province != addressFrmDb.Province)
                {
                    addressFrmDb.Province = address.Province;
                }
                if (!String.IsNullOrEmpty(address.District) && address.District != addressFrmDb.District)
                {
                    addressFrmDb.District = address.District;
                }
                if (!String.IsNullOrEmpty(address.Ward) && address.Ward != addressFrmDb.Ward)
                {
                    addressFrmDb.Ward = address.Ward;
                }
                if (!String.IsNullOrEmpty(address.DetailAddress) && address.DetailAddress != addressFrmDb.DetailAddress)
                {
                    addressFrmDb.DetailAddress = address.DetailAddress;
                }
                if (address.IsDefault && address.IsDefault != addressFrmDb.IsDefault) 
                {
                    await UpdateDefaultAddressAsync(address.PatientId, address.Id);
                    addressFrmDb.IsDefault = true;
                }
             

            await _unitOfWork.SaveChangesAsync();
                return addressFrmDb.Id;
        }
        private async Task UpdateDefaultAddressAsync(Guid PatientId, Guid addressid)
        {
            var patientFromDb =  _unitOfWork.Patients.GetById(PatientId);
            var defaultAddress = patientFromDb.Addresses.FirstOrDefault(a => a.IsDefault);

            if (defaultAddress is not  null && defaultAddress.Id != addressid)
            {
                defaultAddress.IsDefault = false;
                await _unitOfWork.SaveChangesAsync();
            }
            else if (defaultAddress == null)
            {
                // Log the situation or handle it according to business requirements
                throw new InvalidOperationException("No default address set when trying to update.");
            }

        }
    }
}
