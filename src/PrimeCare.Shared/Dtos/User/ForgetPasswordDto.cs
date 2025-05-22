using System.ComponentModel.DataAnnotations;

public class ForgetPasswordDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
}
