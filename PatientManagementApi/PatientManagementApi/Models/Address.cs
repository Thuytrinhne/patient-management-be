using PatientManagementApi.Enums;

namespace PatientManagementApi.Models
{
    public class Address : Entity
    {
        public Guid PatientId { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string DetailAddress { get; set; }
        public bool IsDefault { get; set; }

        // Navigation property
        public Patient Patient { get; set; }
    }
}
