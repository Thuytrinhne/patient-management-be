
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using PatientManagementApi.Dtos.Auth;
using System.Globalization;
using PatientManagementApi.Services;
using PatientManagementApi.Core;

namespace PatientManagementApi.Extensions.DatabaseExtensions
{
    public static class DatabaseExtensions 
    {

        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var serviceProvider = scope.ServiceProvider;

            context.Database.MigrateAsync().GetAwaiter().GetResult(); // auto migration 
            await SeedAsync(context, serviceProvider);
        }
        private static async Task SeedAsync(AppDbContext context, IServiceProvider serviceProvider)
        {
            var csvService = serviceProvider.GetRequiredService<ICsvService>();

            await SeedUsersAsync(context);
            await SeedRolesAsync(context);
            //await SeedPatientsFrmCsvAsync(context, csvService);
            //await SeedAddressesFrmCsvAsync(context, csvService);
            ////await SeedContactInforsFrmCsvAsync(context);

        }

        private static Task SeedContactInforsFrmCsvAsync(AppDbContext context)
        {
            throw new NotImplementedException();
        }

        private static async  Task SeedAddressesFrmCsvAsync(AppDbContext context, ICsvService csvService)
        {
            if (!await context.Addresses.AnyAsync())
            {

                var addresses = csvService.ReadEntitiesFromCsv<Address>(@"D:\patient-management-be\PatientManagementApi\PatientManagementApi\Extensions\DatabaseExtensions\patients.csv");
                await context.Addresses.AddRangeAsync(addresses);
                await context.SaveChangesAsync();
            }
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
        private static async Task SeedPatientsFrmCsvAsync(AppDbContext context, ICsvService csvService)
        {
            if (!await context.Patients.AnyAsync())
            {




                var patients = csvService.ReadEntitiesFromCsv<Patient>(@"D:\patient-management-be\PatientManagementApi\PatientManagementApi\Extensions\DatabaseExtensions\patients.csv");
                await context.Patients.AddRangeAsync(patients);
                await context.SaveChangesAsync();


            }
        }
    
    }
}
