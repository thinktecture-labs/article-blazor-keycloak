using System.Reflection;
using System.Text;
using System.Text.Json;
using Blazor.Keycloak.Api.Models;
using Blazor.Keycloak.Shared.Models;

namespace Blazor.Keycloak.Api.Services
{
    public class ContributionsService
    {
        private static Root _root;

        public Task InitAsync()
        {
            return LoadDataAsync();
        }

        public async Task<List<Contribution>> GetContributionsAsync()
        {
            return _root.Contributions;
        }

        public Contribution GetContribution(int id, CancellationToken cancellationToken)
        {
            var contribution = _root?.Contributions.FirstOrDefault(c => c.Id == id);
            return contribution;
        }

        public void AddContribution(Contribution contribution)
        {
            _root.Contributions.Add(contribution);
        }

        public void UpdateContribution(Contribution contribution)
        {
            _root.Contributions = _root.Contributions.Select((x, i) => x.Id == contribution.Id ? contribution : x).ToList();
        }

        public void RemoveContribution(int contributionId)
        {
            var item = _root.Contributions.FirstOrDefault(c => c.Id == contributionId);
            if (item != null)
            {
                _root.Contributions.Remove(item);
            }
        }

        private async Task LoadDataAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly?.GetManifestResourceStream("Blazor.Keycloak.Api.SampleData.contributions.json");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();
            _root = JsonSerializer.Deserialize<Root>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var index = 1;
            _root.Contributions.ForEach(c => 
                {
                    c.Id = index;
                    index++;
                }
            );
        }
    }
}