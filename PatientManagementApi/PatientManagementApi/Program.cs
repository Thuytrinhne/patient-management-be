using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionOptions = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;
    options.UseNpgsql(connectionOptions.PostgresConstr);
});
builder.Services
  .Configure<ConnectionStringOptions>(builder.Configuration.GetSection(ConnectionStringOptions.Key));

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IContactInforService, ContactInforService>();


builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IContactInforRepository, ContactInforRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    };
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});
builder.Services.AddValidatorsFromAssembly(assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
