using ContactListApi;
using Microsoft.EntityFrameworkCore;
using ContactListApi.Data;
using System.Reflection;
using ContactListApi.Services;
using NLog.Web;
using NLog;
using ContactListApi.Middleware;
using Microsoft.AspNetCore.Identity;
using ContactListApi.Entities;
using FluentValidation;
using ContactListApi.Models;
using ContactListApi.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var authenticationSettings = new AuthenticationSettings();
var builder = WebApplication.CreateBuilder(args);
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddScoped<Seeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped <IValidator<RegisterAppUserDto>, RegisterAppUserDtoValidator>();
builder.Services.AddScoped<IValidator<CreateContactDto>, CreateContactDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateContactDto>, UpdateContactDtoValidator>();
builder.Services.AddScoped<IValidator<ContactQuery>, ContactQueryValidator>();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>
        policyBuilder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(builder.Configuration["AllowedOrigins"])
    );
});

builder.Logging.ClearProviders();

builder.Host.UseNLog();

var app = builder.Build();

app.UseCors("FrontEndClient");
// Seed data
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact API");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
