using PatientManagementApi.Models;
using PatientManagementApi.Services.IServices;

namespace PatientManagementApi.Services
{
    public class AddressService (ICacheService _cacheService, IUnitOfWork _unitOfWork) : IAddressService
    {
        public async Task<Guid> AddAddressAsync(Address address)
        {
            Patient patientFrmDb = await  _unitOfWork.Patients.GetPatientWithAddressesAndContactInfors(address.PatientId);
            if (patientFrmDb is null)
                throw new NotFoundException("A patient not found");
            
            if (patientFrmDb.Addresses.Count <2)
            {
                await _unitOfWork.Addresses.AddAsync(address);
                if (address.IsDefault)
                {
                   await UpdateDefaultAddressAsync(address.PatientId, address.Id);
                }
                await _unitOfWork.SaveChangesAsync();
                await _cacheService.DeleteData(address.PatientId.ToString());
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
            await _cacheService.DeleteData(addressFrmDb.PatientId.ToString());
            await _unitOfWork.SaveChangesAsync();


        }

        private async  Task<bool> IsRemainDefaultAddress(Guid PatientId, Guid addressid)
        {
            var otherAddresses = await  _unitOfWork.Addresses.GetAllAsync(a => a.Id != addressid && a.PatientId ==PatientId);
            if (!otherAddresses.Any())
            {
                return false;
            }
            else
            {
                otherAddresses.First().IsDefault = true;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public Address GetAddressById(Guid id)
        {
            var result =  _unitOfWork.Addresses.GetById(id);

            if (result is null)
                throw new NotFoundException($"Address with Id {id} not found.");

            return result;

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
                throw new NotFoundException($"Address with Id {address.Id} not found.");
            }
            bool updated = false;
            if (!address.IsDefault && address.IsDefault != addressFrmDb.IsDefault)
            {

                if (await IsRemainDefaultAddress(addressFrmDb.PatientId, addressFrmDb.Id))
                {
                    addressFrmDb.IsDefault = false;
                    updated = true;
                }
                else
                {
                    throw new BadRequestException("Cannot update to the undefault address as there are no other addresses available.");

                }
            }
             if (address.IsDefault && address.IsDefault != addressFrmDb.IsDefault) 
                {
                    await UpdateDefaultAddressAsync(addressFrmDb.PatientId, addressFrmDb.Id);
                    addressFrmDb.IsDefault = true;
                    updated = true;
                }
                if (!String.IsNullOrEmpty(address.Province) && address.Province != addressFrmDb.Province)
                {
                    addressFrmDb.Province = address.Province;
                    updated = true;
                }
                if (!String.IsNullOrEmpty(address.District) && address.District != addressFrmDb.District)
                {
                    addressFrmDb.District = address.District;
                    updated = true;

                 }
                if (!String.IsNullOrEmpty(address.Ward) && address.Ward != addressFrmDb.Ward)
                {
                    addressFrmDb.Ward = address.Ward;
                    updated = true;
                }
                if (!String.IsNullOrEmpty(address.DetailAddress) && address.DetailAddress != addressFrmDb.DetailAddress)
                {
                    addressFrmDb.DetailAddress = address.DetailAddress;
                    updated = true;
                }
               


            if (updated)
            {
                await _unitOfWork.SaveChangesAsync();
                await _cacheService.DeleteData(addressFrmDb.PatientId.ToString());

            }
            return addressFrmDb.Id;
        }
       
        private async Task UpdateDefaultAddressAsync(Guid PatientId, Guid addressid)
        {
            var addressesFromDb = await  _unitOfWork.Addresses.GetAllAsync(a=>a.PatientId==PatientId);

            var defaultAddress = addressesFromDb.FirstOrDefault(a => a.IsDefault);

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
