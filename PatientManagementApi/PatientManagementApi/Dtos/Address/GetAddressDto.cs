namespace PatientManagementApi.Dtos
{
    public class GetAddressDto
    {
        public Guid Id { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string DetailAddress { get; set; }
        public bool IsDefault { get; set; }
        public Guid PatientId { get; set; }
    }
}
