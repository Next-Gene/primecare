namespace PrimeCare.Shared;

/// <summary>
/// Represents a standard service response with a success status and a message.
/// </summary>
/// <param name="Success">
/// Indicates whether the service operation was successful.
/// </param>
/// <param name="Message">
/// Provides a message related to the service operation.
/// </param>
public record ServiceResponse(
    /// <summary>
    /// Indicates whether the service operation was successful.
    /// </summary>
    bool Success = false,
    /// <summary>
    /// Provides a message related to the service operation.
    /// </summary>
    string Message = null!
);
