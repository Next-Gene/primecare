namespace PrimeCare.Shared.Dtos.User
{
    // This Data Transfer Object (DTO) is used when a user requests to reset their password.
    public class ForgetPasswordDto
    {
        // The email address of the user who wants to reset their password
        public string Email { get; set; }
    }
}
