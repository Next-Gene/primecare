using Microsoft.AspNetCore.Mvc;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

/// <summary>
/// Controller for handling error responses and returning standardized API error objects.
/// </summary>
[Route("error/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseApiController
{
    /// <summary>
    /// Handles error responses for the specified HTTP status code.
    /// </summary>
    /// <param name="code">The HTTP status code for the error.</param>
    /// <returns>An <see cref="ObjectResult"/> containing an <see cref="ApiResponse"/> with the error code.</returns>
    public IActionResult Error(int code)
    {
        return new ObjectResult(new ApiResponse(code));
    }
}
