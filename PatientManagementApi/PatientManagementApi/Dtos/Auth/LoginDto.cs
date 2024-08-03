using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PatientManagementApi.Dtos.Auth
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("{PropertyName} must be not empty.")
               .EmailAddress().WithMessage("{PropertyName} must be an email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} must be not empty.");
                
        }
    }
}
