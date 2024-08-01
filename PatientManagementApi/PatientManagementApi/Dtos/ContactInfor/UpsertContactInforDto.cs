using System.Text.RegularExpressions;

namespace PatientManagementApi.Dtos.ContactInfor
{
    public class UpsertContactInforDto
    {
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
    public class ContactInforValidator : AbstractValidator<UpsertContactInforDto>
    {
        private static readonly Regex PhoneNumberRegex = new Regex(@"^(?:\+84|0)(?:[3-9]\d{8})$", RegexOptions.Compiled);
        public ContactInforValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("{PropertyName} must be 'Phone or 0' or  'Email or 1' .")
                .NotNull()
                .WithMessage("{PropertyName} must be not null.");

            RuleFor(x => x.Value)
                .NotNull()
                .WithMessage("{PropertyName} must be not null.")
                .NotEmpty()
                .WithMessage("{PropertyName} must be not empty.");

            When(x => x.Type == ContactType.Email, () =>
            {
                RuleFor(x => x.Value).EmailAddress().WithMessage("{PropertyName} must be an email format.");
            }).Otherwise(() =>
            {
                RuleFor(x => x.Value).Matches(PhoneNumberRegex).WithMessage("{PropertyName} must be a valid Vietnamese phone number.");
            });


        }
    }
}
