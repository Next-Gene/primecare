using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PrimeCare.Api.Controllers
{
    /// <summary>
    /// Serves as the base class for all API controllers in the PrimeCare application.
    /// Provides common configuration such as routing and API controller attributes.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
