using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazor.Keycloak.Api.Models;
using Blazor.Keycloak.Shared.Models;

namespace Blazor.Keycloak.Api.Services
{
    public class SpeakerService
    {
        private static Root _root;

        public Task InitAsync()
        {
            return LoadDataAsync();
        }

        public async Task<List<Speaker>> GetSpeakersAsync()
        {
            return _root.Speaker;
        }

        public async Task<Speaker> GetSpeakerAsync(int id, CancellationToken cancellationToken)
        {
            var conf = _root?.Speaker.FirstOrDefault(c => c.Id == id);
            return conf;
        }

        public Speaker GetSpeaker(int id, CancellationToken cancellationToken)
        {
            var speaker = _root?.Speaker.FirstOrDefault(c => c.Id == id);
            return speaker;
        }

        public void AddSpeaker(Speaker speaker)
        {
            _root.Speaker.Add(speaker);
        }

        public void UpdateSpeaker(Speaker speaker)
        {
            _root.Speaker = _root.Speaker.Select((x, i) => x.Id == speaker.Id ? speaker : x).ToList();
        }

        public void RemoveSpeaker(int speakerId)
        {
            var item = _root.Speaker.FirstOrDefault(c => c.Id == speakerId);
            if (item != null)
            {
                _root.Speaker.Remove(item);
            }
        }

        private async Task LoadDataAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly?.GetManifestResourceStream("Blazor.Keycloak.Api.SampleData.speaker.json");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();
            _root = JsonSerializer.Deserialize<Root>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}