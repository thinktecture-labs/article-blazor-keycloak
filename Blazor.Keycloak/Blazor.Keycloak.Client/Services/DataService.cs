
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
        private static IEnumerable<Speaker>? _speakers;

        public DataService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<ICollection<Contribution>> GetContributionsAsync(string searchTerm = "", int skip = 0, int take = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            return (await GetCollectionAsync<Contribution>($"contributions?searchTerm={searchTerm}&skip={skip}&take={take}", cancellationToken)).ToList();
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
            _speakers = await GetCollectionAsync<Speaker>("speakers", cancellationToken);
            return _speakers.ToList();
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