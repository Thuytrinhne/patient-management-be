namespace PatientManagementApi.Dtos.Patient
{
    public class CreatePatientRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<CreateContactInforDto> ContactInfors { get; set; }
        public ICollection<CreateAddressDto> Addresses { get; set; }
    }
    public class PatientValidator : AbstractValidator<CreatePatientRequestDto>
    {
        public PatientValidator()
        {
            RuleFor(x => x.FirstName).Length(1, 50);
            RuleFor(x => x.LastName).Length(1, 50);
            RuleFor(x => x.Gender).NotEmpty().IsInEnum();
            RuleFor(x => x.DateOfBirth).NotEmpty();
        }
    }
}
