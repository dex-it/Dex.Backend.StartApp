using Microsoft.AspNetCore.Mvc;

namespace Server.Web.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : Controller
    {
    }   
}