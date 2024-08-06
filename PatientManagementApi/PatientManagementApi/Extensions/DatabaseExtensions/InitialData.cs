using System.Xml.Linq;

namespace PatientManagementApi.Extensions.DatabaseExtensions
{
    public static class InitialData
    {

        public static IEnumerable<ApplicationRole> Roles => new List<ApplicationRole>()
            {
                new ApplicationRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"

                },
                new ApplicationRole
                {
                    Name = "employee",
                    NormalizedName = "EMPLOYEE"
                }
            };


        public static IEnumerable<ApplicationUser> Users => new List<ApplicationUser>() {

                 new ApplicationUser
                    {
                    UserName = "21522719@gm.uit.edu.vn",
                    Email = "21522719@gm.uit.edu.vn",
                    FirstName = "Thùy",
                    LastName="Trinh",
                    NormalizedEmail="21522719@GM.UIT.EDU.VN",
                    NormalizedUserName="21522719@GM.UIT.EDU.VN",
                    SecurityStamp= "CZTDKWZ5NTBFGTHCX2GQKF3DAFX3DXBS",
                    ConcurrencyStamp="962fffd4-daac-4dff-afe1-213834e71e51",
                    PasswordHash =new PasswordHasher<ApplicationUser>().HashPassword(null, "Rj123456@")
                    }
            };
        
        
    }
}
