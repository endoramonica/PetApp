using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pet.AdminWeb.Apis;
using Pet.AdminWeb.Services;
using Pet.Core.Application.Constants;
using Pet.Core.Application.Infrastructure;
using Refit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Đăng ký HttpContextAccessor để dùng trong AuthRetryHandler
builder.Services.AddHttpContextAccessor();

// Đăng ký AuthRetryHandler
builder.Services.AddTransient<AuthRetryHandler>();

// Đăng ký AuthService nếu có
builder.Services.AddScoped<Pet.AdminWeb.Services.AuthService> ();

// Cấu hình xác thực: Cookie để login Web, JWT để gọi API
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "MyCookieAuth";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Home/Error";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // Cho phép lấy JWT từ cookie khi gửi request nội bộ
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AuthToken"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Cấu hình Refit cho các API cần gọi từ AdminWeb
ConfigureRefit(builder.Services);

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Bắt buộc phải gọi trước Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ==================== Hàm cấu hình Refit ====================
static void ConfigureRefit(IServiceCollection services)
{
    services.AddRefitClient<IUserApi>()
        .AddHttpMessageHandler<AuthRetryHandler>()
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IAuthApi>()
        .AddHttpMessageHandler<AuthRetryHandler>()
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IPhotoUploadService>()
        .AddHttpMessageHandler<AuthRetryHandler>()
        .ConfigureHttpClient(SetHttpClient);

    // Thêm các API khác tùy theo nhu cầu
    void SetHttpClient(HttpClient client)
    {
        client.BaseAddress = new Uri(AppConstants.ApiBaseUrl);
        client.Timeout = TimeSpan.FromSeconds(20);
        Console.WriteLine($"[Refit] BaseAddress set to: {client.BaseAddress}");
    }
}
