using Microsoft.AspNetCore.Components;
using MudBlazor;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;
using avanphamaceuticalsmanagement.Client.Services;

namespace avanphamaceuticalsmanagement.Client.Pages.Management
{
    public partial class DrugManagement
    {
        protected IList<DrugStockTable> drugs = new List<DrugStockTable>();
        protected string searchString1 = string.Empty;
        protected DrugStockTable selectedItem = new();
        protected DrugStockTable stockTable = new();
        protected bool openeditdrawer;
        //Injects
        [Inject]
        IGenericService _genericService { get; set; } 

        [Inject]
        ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
        }

        #region Get Functions
        public async Task GetDrugs()
        {
            var result = await _genericService.GetAllAsync<DrugStockTable>("api/AvanPharmacy/GetDrugs");
            drugs = result.ToList();
        }

        #endregion

        #region Search Function

        private bool FilterFunc1(DrugStockTable element) => FilterFunc(element, searchString1);
        private bool FilterFunc(DrugStockTable element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.DrugName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.DrugCode.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        #endregion

        #region Add and Edit Drugs
        protected async Task SaveDrugs()
        {
            try
            {
                await _genericService.SaveAllAsync("api/AvanPharmacy/PostDrugs", stockTable);
                Snackbar.Add("Succesfully Saved!", Severity.Success);
                stockTable = new();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
                throw;
            }
        }

        protected async Task EditDrug(int id)
        {
            stockTable = drugs.FirstOrDefault(x => x.Id == id);
            openeditdrawer = true;
        }

        protected async Task CompleteEdit()
        {
            try
            {
                await _genericService.UpdateAsync("api/AvanPharmacy/UpdateDrugs", stockTable);
                openeditdrawer = false;
                Snackbar.Add("Successfully Updated!", Severity.Success);
                await GetDrugs();
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                Snackbar.Add(_, Severity.Error);
                throw;
            }
        }

        #endregion
    }
}