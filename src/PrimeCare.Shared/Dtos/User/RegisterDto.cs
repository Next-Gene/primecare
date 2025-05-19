namespace PrimeCare.Shared.Dtos.User
{
    // This Data Transfer Object (DTO) is used when a new user registers an account.
    public class RegisterDto
    {
        // The username chosen by the user (used for login or display)
        public string UserName { get; set; }

        // The user's first name
        public string Fname { get; set; }

        // The user's last name
        public string Lname { get; set; }

        // The user's email address
        public string Email { get; set; }

        // The password the user wants to use
        public string Password { get; set; }

        // Confirmation of the password (should match Password)
        public string Repassword { get; set; }

        // The user's phone number
        public string PhoneNumber { get; set; }
    }
}
