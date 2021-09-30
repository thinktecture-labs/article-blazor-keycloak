using Blazor.Keycloak.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConferencesController : Controller
    {
        private readonly ConferencesService _conferencesService;

        public ConferencesController(ConferencesService conferencesService)
        {
            _conferencesService = conferencesService ?? throw new ArgumentNullException(nameof(conferencesService));
        }

        [HttpGet]
        public async Task<IActionResult> GetConferencesAsync([FromQuery] int skip = 0, [FromQuery] int take = 100,
            CancellationToken cancellationToken = default)
        {
            var conferences = await _conferencesService.GetConferencesAsync();
            var result = conferences.Skip(skip).Take(take);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConferenceAsync([FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var conf = await _conferencesService.GetConferenceAsync(id, cancellationToken);
            if (conf == null)
            {
                return new NotFoundResult();
            }

            return Ok(conf);
        }
    }
}