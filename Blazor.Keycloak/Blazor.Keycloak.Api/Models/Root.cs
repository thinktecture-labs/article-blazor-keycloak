using Blazor.Keycloak.Shared.Models;

namespace Blazor.Keycloak.Api.Models
{
    public class Root
    {
        public List<Contribution> Contributions { get; set; }
        public List<Speaker> Speaker { get; set; }
        public int ItemCount { get; set; }
        public List<Conference> Conferences { get; set; }
    }
}