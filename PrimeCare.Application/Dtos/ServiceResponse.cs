namespace PrimeCare.Application.Dtos;

/// <summary>
/// Represents a service response with a success status and a message.
/// </summary>
/// <param name="Success">Indicates whether the service operation was successful.</param>
/// <param name="Message">Provides a message related to the service operation.</param>
public record ServiceResponse(bool Success = false, string Message = null!);