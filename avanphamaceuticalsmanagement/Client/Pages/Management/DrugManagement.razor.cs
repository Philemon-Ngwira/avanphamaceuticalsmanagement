using Microsoft.AspNetCore.Components;
using MudBlazor;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;
using avanphamaceuticalsmanagement.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace avanphamaceuticalsmanagement.Client.Pages.Management
{
    public partial class DrugManagement
    {
        protected IList<DrugStockTable> drugs = new List<DrugStockTable>();
        protected IList<StockCategoryTable> categories = new List<StockCategoryTable>();
        protected IList<Drugcategory> drugcategories = new List<Drugcategory>();
        protected IList<CosmeticsStockTable> _cosmeticsStocks = new List<CosmeticsStockTable>();
        protected string searchString1 = string.Empty;
        protected DrugStockTable selectedItem = new();
        protected DrugStockTable stockTable = new();
        protected DrugStockTable RequestedDrug = new();
        protected Drugcategory drugcategory = new();
        protected CosmeticsStockTable Cosmetics = new();
        protected StockCategoryTable stockCategoryTable = new();
        protected RestockRequestsTable Request = new();
        protected bool openeditdrawer;
        protected string DrugName = string.Empty;
        protected string DrugCategoryName = string.Empty;
        protected int DrugCategoryId;
        protected double? price;
        protected int QuantityRequest = 0;
        [Inject] AuthenticationStateProvider AuthenticationState { get; set; }
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
        protected async Task GetCategories()
        {
            var result = await _genericService.GetAllAsync<StockCategoryTable>("api/AvanPharmacy/GetCategories");
            categories = result.ToList();
        }
        #endregion
        protected async Task GetDetailsForRequest()
        {
            await GetCategories();
        }
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

        protected async Task FindSelectedCategory()
        {
            if (stockCategoryTable.StockCategoryName == "Medicine")
            {
                Cosmetics = new();
                var res = await _genericService.GetAllAsync<Drugcategory>("api/AvanPharmacy/GetDrugsCategories");
                drugcategories = res.ToList();

            }
            else if (stockCategoryTable.StockCategoryName == "Cosmetics")
            {
                drugcategory = new();
                RequestedDrug = new();
                var res = await _genericService.GetAllAsync<CosmeticsStockTable>("api/AvanPharmacy/GetCosmetics");
                _cosmeticsStocks = res.ToList();
            }
        }

        protected async Task OnDrugCateforySelected()
        {
            drugs.Clear();
            DrugName = string.Empty;
            DrugCategoryId = drugcategory.Id;
            DrugCategoryName = drugcategories.FirstOrDefault(s => s.Id == DrugCategoryId).DrugCategoryName;
            var res = await _genericService.GetAllAsync<DrugStockTable>($"api/AvanPharmacy/GetDrugsByDrugCategory/{DrugCategoryId}");
            drugs = res.ToList();
            StateHasChanged();
        }

        protected async Task RequestQuantityEntered(int value)
        {
            QuantityRequest = value;
            if (RequestedDrug.Quantity != null)
            {
                price = QuantityRequest * RequestedDrug.OrderPrice;
            }
            else if (Cosmetics.Quantity != null)
            {
                price = QuantityRequest * Cosmetics.Price;
            }
        }

        protected async Task SaveRestockRequest()
        {
            if (RequestedDrug.DrugName != null)
            {
                Request.RequestDrugId = RequestedDrug.Id;
                Request.RequestDrugCategoryId = RequestedDrug.DrugCat;
            }
            else if (Cosmetics.CosmeticName != null)
            {
                Request.RequestCosmeticId = Cosmetics.Id;
            }
            Request.OrderPrice = price;
            Request.Quantity = QuantityRequest;
            Request.RequestCategoryId = stockCategoryTable.Id;
            Request.Status = 2;
            var auth =  await AuthenticationState.GetAuthenticationStateAsync();
            var user = auth.User;
            Request.Requester = user.Identity.Name;
            await _genericService.SaveAllAsync("api/AvanPharmacy/SaveRestockRequest", Request);
            Snackbar.Add("Restock Request Successfully Submitted", Severity.Success);
            Request = new();
            RequestedDrug =new();
            Cosmetics = new();
            

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

            public async Task DeleteDrugs(int id)
            {
                try
                {
                    await _genericService.DeleteAsync($"api/AvanPharmacy/DeleteDrugs/{id}");
                    Snackbar.Add("Record Successfully Removed", Severity.Warning);
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

            #region string Functions
        protected Func<StockCategoryTable, string> Categoryconverter = p => p.StockCategoryName;
        protected Func<Drugcategory, string> DrugCategoryconverter = p => p.DrugCategoryName;
        protected Func<DrugStockTable, string> Drugconverter = p => p.DrugName;
        protected Func<CosmeticsStockTable, string> Cosmeticconverter = p => p.CosmeticName;

        #endregion
    }
}