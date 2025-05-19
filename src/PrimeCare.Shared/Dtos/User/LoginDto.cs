namespace PrimeCare.Shared.Dtos.User
{
    // This Data Transfer Object (DTO) is used when a user tries to log in.
    public class LoginDto
    {
        // The user's email address used for login
        public string Email { get; set; }

        // The user's password used for authentication
        public string Password { get; set; }
    }
}
