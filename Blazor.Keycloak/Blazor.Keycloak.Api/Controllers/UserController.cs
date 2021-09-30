
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Keycloak.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Roles()
        {
            return User.FindAll(c => c.Type == "roles").Select(c => c.Value);
        }
    }
}