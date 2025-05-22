using System.ComponentModel.DataAnnotations;

public class ResetPasswordDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
        ErrorMessage = "Password must contain uppercase, lowercase, number and symbol, and be at least 6 characters.")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}
