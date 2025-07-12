using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pet.Core.Application.DTOs;
using Pet.Core.Application.Infrastructure;
using Pet.Core.Contracts.Response;
using System.Security.Claims;

namespace Pet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Có thể dùng [Authorize(Roles = "Admin")] nếu cần
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoggedInUser>> GetUserById(int id)
        {
            var user = await _userService.GetUserByID(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("paging/role")]
        public async Task<ActionResult<PagedResponse<AppUserDto>>> GetUsersInRolePaging(
            [FromQuery] string role,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = "")
        {
            var result = await _userService.GetAllInRolePaging(role, pageNumber, pageSize, searchTerm ?? "");
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<ActionResult<PagedResponse<AppUserDto>>> GetAllUsersPaging(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = "")
        {
            var result = await _userService.GetAllPaging(pageNumber, pageSize, searchTerm ?? "");
            return Ok(result);
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<List<string>>> GetRoles(int id)
        {
            var roles = await _userService.GetRolesOfUser(id);
            return Ok(roles);
        }

        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AssignRoles(int id, [FromBody] List<string> roles)
        {
            await _userService.AssignRoleToUser(id, roles);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] EditUserRequest request)
        {
            await _userService.EditUser(id, request.FullName, request.Email, request.DateOfBirth, request.NewPassword);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }

        [HttpDelete("{id}/permanent")]
        public async Task<IActionResult> DeleteUserPermanently(int id)
        {
            await _userService.DeleteUserPermanently(id);
            return NoContent();
        }

        [HttpPost("{id}/change-photo")]
        public async Task<IActionResult> ChangePhoto(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            using var stream = file.OpenReadStream();
            await _userService.ChangePhotoAsync(id, stream, file.FileName);
            return Ok("Photo updated successfully");
        }

        [HttpGet("{id}/notifications")]
        public async Task<ActionResult<PagedResponse<NotificationDto>>> GetNotifications(
            int id,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetNotificationsAsync(id, pageNumber, pageSize);
            return Ok(result);
        }
    }

    
}
