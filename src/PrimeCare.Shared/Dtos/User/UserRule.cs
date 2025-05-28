using System.ComponentModel.DataAnnotations;

namespace PrimeCare.Shared.Dtos.User
{
    public class UserRoleDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class ChangeUserRoleDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string NewRole { get; set; }
    }
}
