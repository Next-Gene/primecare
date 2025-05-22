using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
