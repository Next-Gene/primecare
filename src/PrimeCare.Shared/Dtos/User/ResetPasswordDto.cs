namespace PrimeCare.Shared.Dtos.User
{
    // This Data Transfer Object (DTO) is used to reset a user's password after verification.
    public class ResetPasswordDto
    {
        // The email address of the user requesting the password reset
        public string Email { get; set; }

        // The new password the user wants to set
        public string Password { get; set; }

        // A confirmation of the new password (should match Password)
        public string ConfirmPassword { get; set; }
    }
}
