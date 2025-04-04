using Microsoft.AspNetCore.Mvc;
using PrimeCare.Infrastructure.Data;

namespace PrimeCare.Api.Controllers;

public class BuggyController : BaseApiController
{
    private readonly PrimeCareContext _context;

    public BuggyController(PrimeCareContext context)
    {
        _context = context;
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        return Ok();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        return Ok();
    }

    [HttpGet("badrequset")]
    public ActionResult GetBadRequsetRequest()
    {
        var thing = _context.Products.Find(42);
        return Ok();
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
        return Ok();
    }
}
