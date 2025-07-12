using Pet.Core.Application.DTOs;
using Pet.Core.Contracts.Response;
using Refit;

namespace Pet.AdminWeb.Apis
{
    [Headers("Authorization: Bearer ")]
    public interface IUserApi
    {
        [Get("/api/user/{id}")]
        Task<LoggedInUser> GetUserById(int id);

        [Get("/api/user/paging/role")]
        Task<PagedResponse<AppUserDto>> GetUsersInRolePaging(
            [Query] string role,
            [Query] int pageNumber = 1,
            [Query] int pageSize = 10,
            [Query] string? searchTerm = "");

        [Get("/api/user/paging")]
        Task<PagedResponse<AppUserDto>> GetAllUsersPaging(
            [Query] int pageNumber = 1,
            [Query] int pageSize = 10,
            [Query] string? searchTerm = "");

        [Get("/api/user/{id}/roles")]
        Task<List<string>> GetRoles(int id);

        [Post("/api/user/{id}/roles")]
        Task AssignRoles(int id, [Body] List<string> roles);

        [Put("/api/user/{id}")]
        Task EditUser(int id, [Body] EditUserRequest request);

        [Delete("/api/user/{id}")]
        Task DeleteUser(int id);

        [Delete("/api/user/{id}/permanent")]
        Task DeleteUserPermanently(int id);

        [Multipart]
        [Post("/api/user/{id}/change-photo")]
        Task<ApiResult> ChangePhotoAsync(
            int id,
            [AliasAs("file")] StreamPart file);

        [Get("/api/user/{id}/notifications")]
        Task<PagedResponse<NotificationDto>> GetNotifications(
            int id,
            [Query] int pageNumber = 1,
            [Query] int pageSize = 10);
    }
}
