var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
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
