namespace PatientManagementApi.Dtos.Patient
{
    public class CreatePatientRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<UpsertContactInforDto> ContactInfors { get; set; }
        public ICollection<UpsertAddressDto> Addresses { get; set; }
    }
    public class CreatePatientValidator : AbstractValidator<CreatePatientRequestDto>
    {
        public CreatePatientValidator()
        {
            RuleFor(x => x.FirstName).Length(1, 50).WithMessage("{PropertyName} must be between 1 and 50 characters.");
            RuleFor(x => x.LastName).Length(1, 50).WithMessage("{PropertyName} must be between 1 and 50 characters.");
            RuleFor(x => x.Gender).NotNull().IsInEnum().WithMessage("{PropertyName} must be specified and valid.");
            RuleFor(x => x.DateOfBirth).NotNull().WithMessage("{PropertyName} must not be null.");

            RuleForEach(x => x.ContactInfors).SetValidator(new ContactInforValidator());

            RuleForEach(x => x.Addresses).SetValidator(new AddressValidator());

        }
    }
}
