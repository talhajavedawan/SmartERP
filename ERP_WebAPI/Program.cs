using AutoMapper;
using ERP_BL.Data;
using ERP_BL.Entities.Core.Roles;
using ERP_BL.Entities.Core.Users;
using ERP_REPO.Repo;
using ERP_REPO.Repo.Authentications;
using ERP_REPO.Repo.Company_Center.Companies;
using ERP_REPO.Repo.CompanyCenter.Companies;
using ERP_REPO.Repo.CompanyCenter.Departments;
using ERP_REPO.Repo.Core.Permissions;
using ERP_REPO.Repo.Core.PowerUsers;
using ERP_REPO.Repo.Core.Roles;
using ERP_REPO.Repo.Core.StatusClasses;
using ERP_REPO.Repo.Core.Statuses;
using ERP_REPO.Repo.Core.Users;
using ERP_REPO.Repo.CustomerCenter;
using ERP_REPO.Repo.HRM.Employees;
using ERP_REPO.Repo.Leaves;
using ERP_REPO.Repo.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


string connectionString;
var dbUser = Environment.GetEnvironmentVariable("DbUser");
var dbPassword = Environment.GetEnvironmentVariable("DbPassword");
var baseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
{
    connectionString = $"{baseConnectionString};User ID={dbUser};Password={dbPassword}";
}
else
{
    connectionString = baseConnectionString;
}
builder.Services.AddAutoMapper(typeof(VendorProfile).Assembly);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });


builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
});


builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<ICompanyRepo, CompanyService>();
builder.Services.AddScoped<ICustomerRepo, CustomerService>();
builder.Services.AddScoped<IUserRepo, UserService>();
builder.Services.AddScoped<IPowerUserRepo, PowerUserService>();
builder.Services.AddScoped<IAuthRepo, AuthService>();
builder.Services.AddScoped<IRoleRepo, RoleService>();
builder.Services.AddScoped<IPermissionRepo, PermissionService>();
builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddScoped<IGroupRepo, GroupService>();
builder.Services.AddScoped<IVendorRepo, VendorService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericService<>));
builder.Services.AddScoped<IVendorNatureRepo, VendorNatureService>();
builder.Services.AddScoped<ICurrencyRepo, CurrencyService>();
builder.Services.AddScoped<ICustomerRepo, CustomerService>();
builder.Services.AddScoped<IVendorContactRepo, VendorContactService>();
builder.Services.AddScoped<IStatusRepo, StatusService>();
builder.Services.AddScoped<IStatusClassRepo, StatusClassService>();
builder.Services.AddScoped<INotificationRepo, NotificationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Application failed to start:");
    Console.WriteLine(ex.Message);
}
