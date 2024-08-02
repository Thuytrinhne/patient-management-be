
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace PatientManagementApi.Services
{
    public class CsvService : ICsvService
    {
        public List<TEntity> ReadEntitiesFromCsv<TEntity>(string filePath) where TEntity : class
        {
          var entities = new List<TEntity>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                MissingFieldFound = null
            };


            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                entities = csv.GetRecords<TEntity>().ToList();
            }

            return entities;
        }
    }
}
