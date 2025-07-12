using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pet.Core.Application.DTOs;
using Pet.Core.Domain.Entities;
namespace Pet.Core.INFRASTRUCTURE.Data.Seed;
public static class RoleInitializer
{
    public static async Task<ApiResult> SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

        string[] roleNames = { "Admin", "User", "Customer" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var role = new AppRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    Description = roleName switch
                    {
                        "Admin" => "Quản trị hệ thống",
                        "User" => "Nhân viên hoặc người dùng nội bộ",
                        "Customer" => "Khách hàng ứng dụng",
                        _ => "Không xác định"
                    }
                };

                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                    return ApiResult.Fail($"Không thể tạo vai trò {roleName}: {errorMessage}");
                }
            }
        }

        return ApiResult.Success();
    }
}
