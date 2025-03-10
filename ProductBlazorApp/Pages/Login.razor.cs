
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using DataServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductBlazorApp.Pages
{
    public partial class Login
    {
        [Inject]
        public IHttpClientService httpService { get; set; }
        [Inject]
        AppState appState { get; set; }

        Token token = null;
        [Inject]
        IToastService toastService { get; set; }

        ViewModels.LoginViewModel model = new ViewModels.LoginViewModel();
        private bool loading;

        protected async override Task OnInitializedAsync()
        {
            token = await httpService.GetTokenAsync();
            if (token != null)
            {
                appState.LoggedIn = true;
                NavigationManager.NavigateTo("/");
            }
        }
        private async void OnValidSubmit()
        {
            loading = true;
            try
            {
                bool login = await httpService.login(model.Username, model.Password);
                if (login)
                {
                    appState.LoggedIn = true;
                    NavigationManager.NavigateTo("/");
                    toastService.ShowInfo($"Successfully logged in", settings => settings.IconType = IconType.None);
                }
                else
                {
                    loading = false;
                    StateHasChanged();
                    toastService.ShowError($"Incorrect User name and/or password You are Not logged in");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                loading = false;
                StateHasChanged();
                toastService.ShowError($"Not logged in");
            }
        }
    }
}
