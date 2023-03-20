
using avanphamaceuticalsmanagement.Client.Pages.Administration.Dialogs;
using avanphamaceuticalsmanagement.Client.Services;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace avanphamaceuticalsmanagement.Client.Pages.Administration
{
    public partial class Admin : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] IRolesService _rolesService { get; set; }
        [Inject] IGenericService _genericService { get; set; }
        [Parameter]
        public UserManager<ApplicationUser> UserManager { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        //[Inject] UserManager<IdentityUser> UserManager { get; set; }
        protected string User { get; set; }
        public string roleName = string.Empty;
        protected UserViewModel UserViewModel { get; set; } = new();
        protected IList<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        List<IdentityUser> usersInSelectedRole = new List<IdentityUser>(); 
        protected IList<string> roles = new List<string>();
        protected string name;
        protected bool _loading;
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            name = user.Identity.Name;
            if (!user.IsInRole("Admin"))
            {
                NavigationManager.NavigateTo("/");
                // Redirect or show an error message
            }
            await GetAllUsersAsync();
            await LoadRoles();
        }

        protected async Task LoadRoles()
        {
            roles = await HttpClient.GetFromJsonAsync<List<string>>("api/Admin/roles");
        }
        protected async Task GetAllUsersAsync()
        {
            if (Users.Count == 0)
            {
                _loading = true;
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetFromJsonAsync<List<ApplicationUser>>("https://localhost:7131/api/Admin");
                    Users = response;

                    _loading = false;

                }
                catch (Exception ex)
                {
                    var _ = ex.Message;
                    throw;
                }
            }
            else
            {
                _loading = false;

            }


        }

        public async Task <bool> SaveProfilePicture(string pictureData, ApplicationUser user)
        {
            // Get the user from the database
           

            if (user == null)
            {
                return false;
            }

            // Save the profile picture to the user object
            user.ProfilePicture = pictureData;

            // Update the user in the database
            try
            {

                var response = await HttpClient.PostAsJsonAsync("api/AdminActions/UpdateProfilePicture", user);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    
                    return false;
                }

            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }

        }

        private async Task EditUserRoles(string id)
        {
            if (id != null)
            {
                var user = Users.Where(u => u.Id == id).FirstOrDefault();
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };
                var parameters = new DialogParameters { ["User"] = user };
                var result = DialogService.Show<EditUserRoles>("Edit User Roles", parameters, options);

                //if (result != null && result.Value)
                //{
                //    await HttpClient.PutAsJsonAsync($"api/Admin/{id}/roles", user.Roles);
                //    await GetAllUsersAsync();
                //}
            }
        }

        protected async Task SaveRole()
        {
            try
            {
                await _rolesService.SaveRole("/api/Admin/SaveAllRoles", roleName);
                Snackbar.Add("Successfully Added", Severity.Success);
                await LoadRoles();
                CloseDialog();
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }


        }
        protected async Task AddRole()
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };
            var result = DialogService.Show<AddRoles>("Add Roles", options);
        }


        protected async Task DeleteUser(string id)
        {
            if (id != null)
            {
                await HttpClient.DeleteAsync($"api/Admin/{id}");
                await GetAllUsersAsync();
            }
        }

        protected void CloseDialog()
        {
            MudDialog.Close();

        }


    }
}