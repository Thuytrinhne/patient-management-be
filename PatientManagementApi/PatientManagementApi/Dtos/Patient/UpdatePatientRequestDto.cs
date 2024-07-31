namespace PatientManagementApi.Dtos.Patient
{
    public class UpdatePatientRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
