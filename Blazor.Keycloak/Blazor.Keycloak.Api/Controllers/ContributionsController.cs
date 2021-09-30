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
    [Authorize]
    public class ContributionsController : Controller
    {
        private readonly ContributionsService _contributionService;

        public ContributionsController(ContributionsService contributionService)
        {
            _contributionService = contributionService ?? throw new ArgumentNullException(nameof(contributionService));
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetContributionsAsync([FromQuery] int skip = 0, [FromQuery] int take = 2000,
            CancellationToken cancellationToken = default)
        {
            var contributions = await _contributionService.GetContributionsAsync();
            Response.Headers["X-Contribution-Count"] = $"{contributions.Count}";
            Response.Headers["Access-Control-Expose-Headers"] = "X-Contribution-Count";
            var result = contributions.Skip(skip).Take(take);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetContribution([FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var contribution = _contributionService.GetContribution(id, cancellationToken);
            if (contribution == null)
            {
                return new NotFoundResult();
            }

            return Ok(contribution);
        }

        [HttpPost]
        public IActionResult CreateContribution([FromBody] Contribution contribution, CancellationToken cancellationToken = default)
        {
            _contributionService.AddContribution(contribution);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContribution([FromRoute]int id, [FromBody] Contribution contribution, CancellationToken cancellationToken = default)
        {
            _contributionService.UpdateContribution(contribution);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteContribution([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            _contributionService.RemoveContribution(id);
            return Ok();
        }
    }
}