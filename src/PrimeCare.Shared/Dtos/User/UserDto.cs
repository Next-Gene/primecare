namespace PrimeCare.Shared.Dtos.User
{
    // This Data Transfer Object (DTO) represents basic user information sent back to the client.
    public class UserDto
    {
        // The user's email address
        public string Email { get; set; }

        // The user's username
        public string UserName { get; set; }

        // The authentication token (e.g., JWT) generated after login or registration
        public string Token { get; set; }
    }
}
