namespace PatientManagementApi.Dtos.Address
{
    public class UpsertAddressDto
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string DetailAddress { get; set; }
        public bool IsDefault { get; set; }
    }
    public class AddressValidator : AbstractValidator<UpsertAddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Province)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(1, 100).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(1, 100).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.Ward)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(1, 100).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.DetailAddress)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(1, 200).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("{PropertyName} must be specified.");
        }
    }
}
