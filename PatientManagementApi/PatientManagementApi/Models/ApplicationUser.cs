namespace PatientManagementApi.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // secretkey
        // xđ refresh
        // middle ware để kt có trùng vs refresh trong db của use đó k 
    }
}
