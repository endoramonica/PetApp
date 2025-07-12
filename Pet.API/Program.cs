using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pet.API.Extensions;
using Pet.API.Services;
using Pet.Core.Application.Infrastructure;
using Pet.Core.Application.ServicesApplication;
using Pet.Core.Domain.Entities;
using Pet.Core.INFRASTRUCTURE.Data;
using Pet.Core.INFRASTRUCTURE.Data.Seed;
using Pet.Core.INFRASTRUCTURE.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// JWT Settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];
var secretKey = jwtSettings["SecretKey"] ?? jwtSettings["Key"];
var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey
        };
    });

// Authorization
builder.Services.AddAuthorization();

// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.HandshakeTimeout = TimeSpan.FromSeconds(15);
});

// Antiforgery + HttpContext
builder.Services.AddAntiforgery();
builder.Services.AddHttpContextAccessor();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Pet Adoption API",
        Version = "v1"
    });

    var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description = "Nhập token JWT của bạn vào đây (Bearer {token})",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// EF Core
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// IdentityCore cho Web API
builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddRoles<AppRole>()
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddSignInManager<SignInManager<AppUser>>()
.AddUserManager<UserManager<AppUser>>()
.AddRoleManager<RoleManager<AppRole>>()
.AddDefaultTokenProviders();
// MongoDB
builder.Services.AddSingleton<MongoDBContext>(sp =>
    new MongoDBContext(
        builder.Configuration["MongoDB:ConnectionString"],
        builder.Configuration["MongoDB:DatabaseName"]
    ));

// AutoMapper
builder.Services.AddMapper();
// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPhotoUploadService, PhotoUploadService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    ApplyDBMigrations(app.Services);
    using (var scope = app.Services.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        await RoleInitializer.SeedRolesAsync(scopedProvider); // ✅ Seed Role đúng cách
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void ApplyDBMigrations(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}
