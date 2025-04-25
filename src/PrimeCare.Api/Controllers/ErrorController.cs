using Microsoft.AspNetCore.Mvc;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

[Route("error/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseApiController
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new ApiResponse(code));
    }
}
