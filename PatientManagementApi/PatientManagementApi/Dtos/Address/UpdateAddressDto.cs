namespace PatientManagementApi.Dtos.Address
{
    public class UpdateAddressDto
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string DetailAddress { get; set; }
        public bool IsDefault { get; set; }
    }
}
