using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_]{3,20}$", ErrorMessage = "Username must be 3-20 characters, letters, numbers or underscores only.")]
    public string UserName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be at least 3 characters.")]
    public string Fname { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be at least 3 characters.")]
    public string Lname { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
        ErrorMessage = "Password must contain uppercase, lowercase, number and symbol, and be at least 6 characters.")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string Repassword { get; set; }

    [Required]
    [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format")]
    public string PhoneNumber { get; set; }
}
