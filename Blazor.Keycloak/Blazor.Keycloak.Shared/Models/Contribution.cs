using System.Collections.Generic;

namespace Blazor.Keycloak.Shared.Models
{
    public class Contribution
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Language { get; set; }
        public string Abstract { get; set; }
        public HashSet<int> Speaker { get; set; }
        public List<string> Tags { get; set; }
        public string PrimaryTag { get; set; }
        public string Time { get; set; }
        public string AdditionalInfo { get; set; }
        public int Conference { get; set; }
    }
}