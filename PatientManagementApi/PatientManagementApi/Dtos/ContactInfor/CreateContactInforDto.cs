using PatientManagementApi.Enums;
using System.Text.RegularExpressions;

namespace PatientManagementApi.Dtos.ContactInfor
{
    public class CreateContactInforDto
    {
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
    public class ContactInforValidator : AbstractValidator<CreateContactInforDto>
    {
        private static readonly Regex PhoneNumberRegex = new Regex(@"^(?:\+84|0)(?:[3-9]\d{8})$", RegexOptions.Compiled);

        public ContactInforValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum()
                .NotEmpty()
                .WithMessage("Contact type must be specified.");

            When(x => x.Type == ContactType.Email, () =>
            {
                RuleFor(x => x.Value).NotEmpty();
                RuleFor(x => x.Value).EmailAddress();
            }).Otherwise(() =>
            {
                RuleFor(x => x.Value).NotEmpty();
                RuleFor(x => x.Value).Matches(PhoneNumberRegex);
            });

            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage("Value is required.")
                .Matches(PhoneNumberRegex)
                .WithMessage("Value must be a valid Vietnamese phone number.")
                .When(x => x.Type == ContactType.Phone);
        }
    }
}
