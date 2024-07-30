using PatientManagementApi.Enums;

namespace PatientManagementApi.Models
{
    public class ContactInfor : Entity
    {
        public Guid  PatientId { get; set; }
        public ContactType Type { get; set; } // 'Phone' or 'Email'
        public string Value { get; set; }

        // Navigation property
        public Patient Patient { get; set; }
    }
}
