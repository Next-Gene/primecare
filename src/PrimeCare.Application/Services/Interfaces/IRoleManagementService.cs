using PrimeCare.Shared.Dtos.User;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IRoleManagementService
    {
        Task<UserRoleDto> GetUserRolesAsync(string email);
        Task<bool> ChangeUserRoleAsync(string email, string newRole);
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<bool> AssignDefaultRoleToUserAsync(string email);
    }
}
