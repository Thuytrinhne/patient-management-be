using PatientManagementApi.Dtos.ContactInfor;
using PatientManagementApi.Enums;

namespace PatientManagementApi.Dtos.Patient
{
    public class GetPatientsResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string DeactivationReason { get; set; }
        public DateTime? DeactivatedAt { get; set; }

        public ICollection<GetContactInforDto> ContactInfors { get; set; }
        public ICollection<GetAddressDto> Addresses { get; set; }
    }
}
