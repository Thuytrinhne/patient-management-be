namespace PatientManagementApi.Dtos.Patient
{
    public class UpdatePatientRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    public class UpdatePatientValidator : AbstractValidator<UpdatePatientRequestDto>
    {
        public UpdatePatientValidator()
        {
            RuleFor(x => x.FirstName).Length(1, 50).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");
            RuleFor(x => x.LastName).Length(1, 50).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");
            RuleFor(x => x.Gender).NotNull().IsInEnum().WithMessage("{PropertyName} must be specified and valid.");
            RuleFor(x => x.DateOfBirth).NotNull().WithMessage("{PropertyName} must not be null.");
        
        }
    }


}
