namespace PrimeCare.Application.Errors;

/// <summary>
/// Represents an API validation error response with a status code and a list of validation errors.
/// </summary>
public class ApiValidationErrorResponse : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiValidationErrorResponse"/> class.
    /// </summary>
    public ApiValidationErrorResponse() : base(400)
    {
    }

    /// <summary>
    /// Gets or sets the validation errors.
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = null!;
}