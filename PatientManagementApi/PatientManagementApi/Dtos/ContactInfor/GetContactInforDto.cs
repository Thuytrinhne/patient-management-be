using PatientManagementApi.Enums;

namespace PatientManagementApi.Dtos.ContactInfor
{
    public class GetContactInforDto
    {
        public Guid Id { get; set; }
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
}
