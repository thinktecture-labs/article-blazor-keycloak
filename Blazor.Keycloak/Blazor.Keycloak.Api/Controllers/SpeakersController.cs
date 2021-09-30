using Blazor.Keycloak.Api.Services;
using Blazor.Keycloak.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpeakersController : Controller
    {
        private readonly SpeakerService _speakerService;

        public SpeakersController(SpeakerService speakerService)
        {
            _speakerService = speakerService ?? throw new ArgumentNullException(nameof(speakerService));
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetSpeakersAsync([FromQuery] int skip = 0, [FromQuery] int take = 100,
            CancellationToken cancellationToken = default)
        {
            var speakers = await _speakerService.GetSpeakersAsync();
            Response.Headers["X-Contribution-Count"] = $"{speakers.Count}";
            Response.Headers["Access-Control-Expose-Headers"] = "X-Contribution-Count";
            var result = speakers.Skip(skip).Take(take);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpeakerAsync([FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var speaker = await _speakerService.GetSpeakerAsync(id, cancellationToken);
            if (speaker == null)
            {
                return new NotFoundResult();
            }

            return Ok(speaker);
        }

        [HttpPost]
        public IActionResult CreateSpeaker([FromBody] Speaker speaker, CancellationToken cancellationToken = default)
        {
            _speakerService.AddSpeaker(speaker);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSpeaker([FromRoute] int id, [FromBody] Speaker speaker, CancellationToken cancellationToken = default)
        {
            _speakerService.UpdateSpeaker(speaker);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteSpeaker([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            _speakerService.RemoveSpeaker(id);
            return Ok();
        }
    }
}