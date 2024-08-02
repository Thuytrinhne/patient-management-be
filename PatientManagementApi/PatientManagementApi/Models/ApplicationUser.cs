namespace PatientManagementApi.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
