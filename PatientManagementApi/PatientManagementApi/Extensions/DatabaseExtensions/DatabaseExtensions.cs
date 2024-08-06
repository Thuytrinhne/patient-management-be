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

            context.Database.MigrateAsync().GetAwaiter().GetResult(); 
            await SeedAsync(context, serviceProvider);
        }
        private static async Task SeedAsync(AppDbContext context, IServiceProvider serviceProvider)
        {
            var csvService = serviceProvider.GetRequiredService<ICsvService>();

            await SeedEntityAsync(context.Users, InitialData.Users, context);
            await SeedEntityAsync(context.Roles, InitialData.Roles, context);
            await SeedEntityFromCsvAsync(context.Patients, csvService, "./Extensions/DatabaseExtensions/CsvFiles/patients_data.csv", context);
            await SeedEntityFromCsvAsync(context.Addresses, csvService, "./Extensions/DatabaseExtensions/CsvFiles/addresses_data.csv", context);
        }

        private static async Task SeedEntityAsync<T>(DbSet<T> dbSet, IEnumerable<T> data, AppDbContext context) where T : class
        {
            if (!await dbSet.AnyAsync())
            {
                await dbSet.AddRangeAsync(data);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedEntityFromCsvAsync<T>(DbSet<T> dbSet, ICsvService csvService, string filePath, AppDbContext context) where T : class
        {
            if (!await dbSet.AnyAsync())
            {
                var entities = csvService.ReadEntitiesFromCsv<T>(filePath);
                await dbSet.AddRangeAsync(entities);
                await context.SaveChangesAsync();
            }
        }

    }
}
