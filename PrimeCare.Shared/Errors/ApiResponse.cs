namespace PrimeCare.Shared.Errors;

/// <summary>
/// Represents an API response with a status code and a message.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse"/> class.
    /// </summary>
    /// <param name="statusCode">The status code of the response.</param>
    /// <param name="message">The message of the response. If null, a default message will be used based on the status code.</param>
    public ApiResponse(int statusCode, string message = null!)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefualtMessageForStatusCode(statusCode);
    }

    /// <summary>
    /// Gets or sets the status code of the response.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the message of the response.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets the default message for a given status code.
    /// </summary>
    /// <param name="statusCode">The status code for which to get the default message.</param>
    /// <returns>The default message for the given status code.</returns>
    private string GetDefualtMessageForStatusCode(int statusCode)
        => statusCode switch
        {
            400 => "A bad request, you have made",
            401 => "Authorized, you are not",
            404 => "Resource found, it was not",
            500 => "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change",
            _ => null!
        };
}