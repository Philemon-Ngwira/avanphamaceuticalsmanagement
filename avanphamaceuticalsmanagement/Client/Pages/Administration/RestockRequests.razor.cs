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
        protected IList<BudgetsTable> budgets = new List<BudgetsTable>();
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
            int currentYear = DateTime.Now.Year;
            RestockRequest = _restockRequests.FirstOrDefault(x => x.Id == id);
            RestockRequest.Status = 1;
            double budget = (double)budgets.Where(x => x.BudgetTypeId == 3 & x.StartDate.Value.Year == currentYear).Select(x => x.Amount).Sum();
            int selectedBudget = budgets.Where(x => x.BudgetTypeId == 3 & x.StartDate.Value.Year == currentYear).Select(x => x.id).FirstOrDefault();
            if (budget > 0)
            {
                var amount = (double)(budget - RestockRequest.OrderPrice);
                if (budget > amount)
                {
                    //Update Current Expense Budget
                    foreach (var item in budgets)
                    {
                        if (item.BudgetTypeId == 3 & item.id == selectedBudget)
                        {
                            item.Amount = amount;
                        }
                    }
                    var virtualProperties = RestockRequest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetGetMethod()?.IsVirtual == true);
                    foreach (var virtualProperty in virtualProperties)
                    {
                        var value = virtualProperty.GetValue(RestockRequest);
                        if (value != null)
                        {
                            virtualProperty.SetValue(RestockRequest, null);
                        }
                    }
                    //Save Expense
                    //UpdateRestockRequest
                    await _genericService.UpdateAsync("api/AvanPharmacy/UpdateRestockRequest", RestockRequest);
                    Snackbar.Add("Request Approved!", Severity.Success);
                    RestockRequest = new();
                    await GetApprovalRequests();
                }
                else
                {
                    Snackbar.Add("Available Budget Not enough for request Adjust Budget", Severity.Error);
                    return;
                }
            }
            else
            {
                Snackbar.Add("No Budget Available for Request", Severity.Error);
                return;
            }
        }
    }
}