using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;

namespace avanphamaceuticalsmanagement.Client.Pages.Administration
{
    public partial class RestockRequests
    {
#region Injects
        [Inject]
        IGenericService _genericService { get; set; }

        [Inject]
        ISnackbar Snackbar { get; set; }

#endregion
        protected IList<RestockRequestsTable> _restockRequests = new List<RestockRequestsTable>();
        protected RestockRequestsTable RestockRequest = new();
        private TableGroupDefinition<RestockRequestsTable> _groupDefinition = new()
        {
            GroupName = "Requester",
            Indentation = false,
            Expandable = true,
            IsInitiallyExpanded = false,
            Selector = (e) => e.Requester
        };
        protected override async Task OnInitializedAsync()
        {
            await GetApprovalRequests();
        }

        protected async Task GetApprovalRequests()
        {
            var requests = await _genericService.GetAllAsync<RestockRequestsTable>("api/AvanPharmacy/GetRestockRequests");
            _restockRequests = requests.ToList();
            List<RestockRequestsTable> restocks = new();
            foreach (var item in _restockRequests)
            {
                if (item.Status == 2)
                {
                    restocks.Add(item);
                }
            }

            _restockRequests = restocks;
        }

        protected async Task HandleRejects(int id)
        {
            RestockRequest = _restockRequests.FirstOrDefault(x => x.Id == id);
            RestockRequest.Status = 3;
            var virtualProperties = RestockRequest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetGetMethod()?.IsVirtual == true);
            foreach (var virtualProperty in virtualProperties)
            {
                var value = virtualProperty.GetValue(RestockRequest);
                if (value != null)
                {
                    virtualProperty.SetValue(RestockRequest, null);
                }
            }

            await _genericService.UpdateAsync("api/AvanPharmacy/UpdateRestockRequest", RestockRequest);
            Snackbar.Add("Request Rejected!", Severity.Error);
            RestockRequest = new();
            await GetApprovalRequests();
        }

        protected async Task HandleApprovals(int id)
        {
            RestockRequest = _restockRequests.FirstOrDefault(x => x.Id == id);
            RestockRequest.Status = 1;
            var virtualProperties = RestockRequest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetGetMethod()?.IsVirtual == true);
            foreach (var virtualProperty in virtualProperties)
            {
                var value = virtualProperty.GetValue(RestockRequest);
                if (value != null)
                {
                    virtualProperty.SetValue(RestockRequest, null);
                }
            }

            await _genericService.UpdateAsync("api/AvanPharmacy/UpdateRestockRequest", RestockRequest);
            Snackbar.Add("Request Approved!", Severity.Success);
            RestockRequest = new();
            await GetApprovalRequests();
        }
    }
}