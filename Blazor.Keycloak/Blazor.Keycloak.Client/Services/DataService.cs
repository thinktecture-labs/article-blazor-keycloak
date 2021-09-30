
using Blazor.Keycloak.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Client.Services
{
    public class DataService
    {
        private readonly HttpClient _client;
        private readonly NavigationManager _navigationManager;
        private static IEnumerable<Contribution> _contributions;
        private static IEnumerable<Speaker> _speakers;

        public DataService(HttpClient client, NavigationManager navigationManager)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        }


        public async Task<ICollection<Contribution>> GetContributionsAsync(string searchTerm = "", int skip = 0, int take = Int32.MaxValue,
            CancellationToken cancellationToken = default)
        {
            return (await GetCollectionAsync<Contribution>($"contributions?searchTerm={searchTerm}&skip={skip}&take={take}", cancellationToken)).ToList();
        }

        public async Task<int> GetContributionCountAsync(CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, $"contributions");

            using var httpResponse = await _client.SendAsync(request, cancellationToken);
            var countHeader = httpResponse.Headers.GetValues("X-Contribution-Count")?.FirstOrDefault();
            var count = 0;
            if (!string.IsNullOrWhiteSpace(countHeader))
            {
                int.TryParse(countHeader, out count);
            }
            Console.WriteLine($"ContributionCount: {count}");
            return count;
        }

        public async Task<Contribution> GetContributionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Load Contribution for Id: {id}");
            return await _client.GetFromJsonAsync<Contribution>($"contributions/{id}", cancellationToken);
        }

        public Task UpdateContribution(Contribution contribution, CancellationToken cancellationToken = default)
        {
            return _client.PutAsJsonAsync<Contribution>($"contributions/{contribution.Id}", contribution, cancellationToken);
        }

        public Task UpdateSpeaker(Speaker speaker, CancellationToken cancellationToken = default)
        {
            return _client.PutAsJsonAsync<Speaker>($"contributions/{speaker.Id}", speaker, cancellationToken);
        }

        public Task RemoveContributionAsync(int contributionId, CancellationToken cancellationToken = default)
        {
            return _client.DeleteAsync($"contributions/{contributionId}", cancellationToken);
        }

        public Task RemoveSpeakerAsync(int speakerId, CancellationToken cancellationToken = default)
        {
            return _client.DeleteAsync($"speakers/{speakerId}", cancellationToken);
        }

        public async Task<ICollection<Speaker>> GetSpeakersAsync(CancellationToken cancellationToken = default)
        {
            if (_speakers == null)
            {
                _speakers = await GetCollectionAsync<Speaker>("speakers", cancellationToken);
            }

            return _speakers.ToList();
        }

        public async Task<string> GetSpeakerByContribution(int id, CancellationToken cancellationToken = default)
        {
            if (_speakers == null)
            {
                _speakers = await GetCollectionAsync<Speaker>("speakers", cancellationToken);
            }

            if (_contributions == null)
            {
                _contributions = await GetCollectionAsync<Contribution>("contributions", cancellationToken);
            }

            var contribution = _contributions.FirstOrDefault(c => c.Id == id);
            if (contribution == null)
            {
                return String.Empty;
            }

            var speakers = _speakers.Where(s => contribution.Speaker.Contains(s.Id));
            return String.Join(", ", speakers.OrderBy(s => s.FirstName).Select(s => $"{s.FirstName} {s.LastName}"));
        }

        private async Task<IEnumerable<T>> GetCollectionAsync<T>(string path,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _client.GetFromJsonAsync<IEnumerable<T>>($"{path}", cancellationToken);
                return result;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized...");
                }
            }
            return new List<T>();
        }
    }
}