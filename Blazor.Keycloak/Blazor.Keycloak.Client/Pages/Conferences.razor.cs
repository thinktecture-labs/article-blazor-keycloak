using Blazor.Keycloak.Client.Services;
using Blazor.Keycloak.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Client.Pages
{
    public partial class Conferences
    {
        [Inject] public DataService DataService { get; set; }

        private bool isLoading = false;
        private ICollection<Contribution> Contributions = new List<Contribution>();

        protected override async Task OnInitializedAsync()
        {
            await LoadContributions();
            await base.OnInitializedAsync();
        }

        private async Task LoadContributions()
        {
            isLoading = true;
            Contributions = await DataService.GetContributionsAsync();
            isLoading = false;
        }

        private void RowClicked()
        {
            Console.WriteLine($"Row clicked...");
        }

        private async Task DeleteContribution(int id)
        {
            await DataService.RemoveContributionAsync(id);
            await InvokeAsync(async () => await LoadContributions());
        }

        private void Cancel()
        {
            Console.WriteLine("Cancel contribution");
        }
    }
}
