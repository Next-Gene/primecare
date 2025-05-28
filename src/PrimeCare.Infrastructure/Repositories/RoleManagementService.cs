using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Shared.Dtos.User;

namespace PrimeCare.Application.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserRoleDto> GetUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserRoleDto
            {
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> ChangeUserRoleAsync(string email, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("User not found");

            // Validate the new role exists
            if (!await _roleManager.RoleExistsAsync(newRole))
                throw new ArgumentException($"Role '{newRole}' does not exist");

            // Get current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove all current roles
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to remove current roles: {errors}");
                }
            }

            // Add the new role
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign new role: {errors}");
            }

            return true;
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<bool> AssignDefaultRoleToUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("User not found");

            // Check if user already has roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                return true; // User already has roles

            // Assign default "User" role
            var result = await _userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign default role: {errors}");
            }

            return true;
        }
    }
}