using PatientManagementApi.Enums;

namespace PatientManagementApi.Dtos
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

        public ICollection<ContactInfor> ContactInfos { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
