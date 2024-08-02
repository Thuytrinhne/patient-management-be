using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PatientManagementApi.ConfigKeys;
using PatientManagementApi.Dtos.Auth;
using PatientManagementApi.Extensions;
using PatientManagementApi.Extensions.DatabaseExtensions;
using PatientManagementApi.Extensions.Exceptions.Handler;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionOptions = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;

    options.UseNpgsql(connectionOptions.PostgresConstr);
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var connectionOptions = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;
    options.Configuration = connectionOptions.RedisConstr;
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection(ConnectionStringOptions.Key));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Key));


// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IContactInforService, ContactInforService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICsvService, CsvService>();


builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IContactInforRepository, ContactInforRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ContactInforValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdatePatientValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreatePatientValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpsertAddressDto>());



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});
builder.AddAppAuthetication();
builder.Services.AddAuthorization();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter(policyName: "fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });

});
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
    });

var app = builder.Build();
app.UseCors("CorsPolicy");




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.UseExceptionHandler(opts => { });

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
