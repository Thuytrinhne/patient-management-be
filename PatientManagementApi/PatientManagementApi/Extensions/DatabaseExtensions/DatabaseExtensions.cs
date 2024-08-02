
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using PatientManagementApi.Dtos.Auth;

namespace PatientManagementApi.Extensions.DatabaseExtensions
{
    public static class DatabaseExtensions
    {

        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult(); // auto migration 
          //  await SeedAsync(context);
        }
        private static async Task SeedAsync(AppDbContext context)
        {
            await SeedUsersAsync(context);
            await SeedRolesAsync(context);

            await SeedPatientsAsync(context);
            //await SeedAddressesAsync(context);
            //await SeedContactInforsAsync(context);

        }

        private static Task SeedContactInforsAsync(AppDbContext context)
        {
            throw new NotImplementedException();
        }

        private static Task SeedAddressesAsync(AppDbContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task SeedUsersAsync(AppDbContext context)
        {

            if (!await context.Users.AnyAsync())
            {        
                await context.Users.AddRangeAsync(InitialData.Users);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedRolesAsync(AppDbContext context)
        {

            if (!await context.Roles.AnyAsync())
            {
              
                await context.Roles.AddRangeAsync(InitialData.Roles);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedPatientsAsync(AppDbContext context)
        {
            if (!await context.Patients.AnyAsync())
            {
                string patientJson = System.IO.File.ReadAllText(@"D:\patient-management-be\PatientManagementApi\PatientManagementApi\Extensions\DatabaseExtensions\Patient.json");
                List<Patient> patients = JsonConvert.DeserializeObject<List<Patient>>(patientJson);
                await context.Patients.AddRangeAsync(patients);
                await context.SaveChangesAsync();



            
            }
        }
    
    }
}
