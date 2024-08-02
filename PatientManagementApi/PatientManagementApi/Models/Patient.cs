using PatientManagementApi.Enums;

namespace PatientManagementApi.Models
{
    public class Patient : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string DeactivationReason { get; set; }
        public DateTime ? DeactivatedAt { get; set; }

        public ICollection<ContactInfor> ContactInfors { get; set; }
        public ICollection<Address> Addresses { get; set; }

        public Patient()
        {
            IsActive = true;
        }

    }


}
