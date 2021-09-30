
using Blazor.Keycloak.Client.Services;
using Blazor.Keycloak.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Client.Pages
{
    public partial class Speakers
    {
        [Inject] public DataService DataService { get; set; }

        private bool isLoading = false;
        private ICollection<Speaker> SpeakerList = new List<Speaker>();

        protected override async Task OnInitializedAsync()
        {
            await LoadSpeakers();
            await base.OnInitializedAsync();
        }


        private void RowClicked()
        {
            Console.WriteLine($"Row clicked...");
        }

        private async Task DeleteSpeaker(int id)
        {
            await DataService.RemoveContributionAsync(id);
            await InvokeAsync(async () => await LoadSpeakers());
        }

        private void Cancel()
        {
            Console.WriteLine("Cancel contribution");
        }

        private async Task LoadSpeakers()
        {
            isLoading = true;
            SpeakerList = await DataService.GetSpeakersAsync();
            isLoading = false;
        }
    }
}