namespace PatientManagementApi.Services.IServices
{
    public interface IAddressService 
    {
        Task<IEnumerable<Address>> GetAllAddressAsync(Guid? PatientId = null);
        Address GetAddressById(Guid id);
        Task<Guid> AddAddressAsync(Address address);
        Task<Guid> UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Guid id);
    }
}
