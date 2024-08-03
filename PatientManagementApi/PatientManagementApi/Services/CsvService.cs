
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CsvHelper.TypeConversion;
using System.Formats.Asn1;

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
                if (typeof(TEntity) == typeof(Patient))
                {
                    csv.Context.RegisterClassMap<CsvPatientMap>();
                }
                else if (typeof(TEntity) == typeof(Address))
                {
                    csv.Context.RegisterClassMap<CsvAddressMap>(); 
                }

                entities = csv.GetRecords<TEntity>().ToList();
            }

            return entities;
        }
    }
    public sealed class CsvAddressMap : ClassMap<Address>
    {
        public CsvAddressMap()
        {
            Map(m => m.Id).Index(0);
            Map(m => m.PatientId).Index(1);
            Map(m => m.Province).Index(2);
            Map(m => m.District).Index(3);
            Map(m => m.Ward).Index(4);
            Map(m => m.DetailAddress).Index(5);
            Map(m => m.IsDefault).Index(6);
        
        }
    }
    public sealed class CsvPatientMap : ClassMap<Patient>
    {
        public CsvPatientMap()
        {
            string[] formats = new string[] { "dd/MM/yyyy H:mm", "M/d/yyyy h:mm:ss tt", "dd/MM/yyyy hh:mm:ss tt", "M/d/yyyy H:mm" };
            var cultureInfo = CultureInfo.InvariantCulture; // Sử dụng InvariantCulture để tránh những vấn đề liên quan đến định dạng vùng

            Map(m => m.Id)        
              .Index(0);

            Map(m => m.FirstName)
            .Index(1);

            Map(m => m.LastName)
           .Index(2);

            Map(m => m.Gender)
            .Index(3);
          
       
            Map(m => m.DateOfBirth)
                .TypeConverterOption.Format(formats)
                .TypeConverterOption.CultureInfo(cultureInfo)
                .Index(4);
            Map(m => m.IsActive)
            .Index(5);

            Map(m => m.DeactivationReason)
            .TypeConverter(new NullStringConverter()) 
            .Index(6);

            Map(m => m.CreatedAt)
                .TypeConverterOption.Format(formats)
                .TypeConverterOption.CultureInfo(cultureInfo)
                .Index(7);
            Map(m => m.CreatedBy)
            .Index(8);

            Map(m => m.UpdatedAt)
               .TypeConverterOption.Format(formats)
               .TypeConverterOption.CultureInfo(cultureInfo)
               .Index(9); 
            Map(m => m.UpdatedBy)
            .Index(10);
            Map(m => m.DeactivatedAt)
              .TypeConverterOption.Format(formats)
              .TypeConverterOption.CultureInfo(cultureInfo)
              .TypeConverter(new NullableDateTimeConverter())
              .Index(11); 
        }
    }
    public class NullableDateTimeConverter : DateTimeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            // Thêm định dạng mới vào danh sách các định dạng được chấp nhận
            var formats = new[] { "dd/MM/yyyy H:mm", "M/d/yyyy h:mm:ss tt", "dd/MM/yyyy hh:mm:ss tt", "M/d/yyyy H:mm" };
            var cultureInfo = CultureInfo.InvariantCulture;

            // Thử phân tích cú pháp ngày giờ với các định dạng được cung cấp
            DateTime dateTime;
            if (DateTime.TryParseExact(text, formats, cultureInfo, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }

            throw new InvalidOperationException($"Unable to parse '{text}' to a valid DateTime.");
        }
    }

    public class NullStringConverter : StringConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == "NULL" || String.IsNullOrEmpty(text))
            {
                return null; 
            }
            return text; 
        }
    }
}
