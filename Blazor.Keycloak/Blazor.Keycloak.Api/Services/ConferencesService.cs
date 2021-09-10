using Blazor.Keycloak.Api.Models;
using Blazor.Keycloak.Shared.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Blazor.Keycloak.Api.Services
{
    public class ConferencesService
    {
        private static Root _root;

        public Task InitAsync()
        {
            return LoadDataAsync();
        }

        public async Task<List<Conference>> GetConferencesAsync()
        {
            return _root.Conferences;
        }

        public async Task<Conference> GetConferenceAsync(int id, CancellationToken cancellationToken)
        {
            var conf = _root?.Conferences.FirstOrDefault(c => c.Id == id);
            return conf;
        }

        private async Task LoadDataAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly?.GetManifestResourceStream("Blazor.Statemanagement.Api.SampleData.conferences.json");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();
            _root = JsonSerializer.Deserialize<Root>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}