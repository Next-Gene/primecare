namespace PrimeCare.Application.Errors;

/// <summary>
/// Represents an API exception with a status code, message, and details.
/// </summary>
public class ApiException : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class.
    /// </summary>
    /// <param name="statusCode">The status code of the exception.</param>
    /// <param name="message">The message of the exception. If null, a default message will be used based on the status code.</param>
    /// <param name="details">The details of the exception.</param>
    public ApiException(int statusCode, string message = null!,
        string details = null!) : base(statusCode, message)
    {
        Details = details;
    }

    /// <summary>
    /// Gets or sets the details of the exception.
    /// </summary>
    public string Details { get; set; }
}