namespace PrimeCare.Shared.Dtos.User
{
    // This Data Transfer Object (DTO) is used to verify the code sent to the user's email during password reset or other verification processes.
    public class VerifyCodeDto
    {
        // The user's email address to identify the account being verified
        public string Email { get; set; }

        // The verification code that the user received (e.g., via email)
        public string Code { get; set; }
    }
}
