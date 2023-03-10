using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;

namespace avanphamaceuticalsmanagement.Client.Pages.Transactions
{
    public partial class SalesTransactions
    {
        [Inject]
        IGenericService GenericService { get; set; }

        [Inject]
        ISnackbar Snackbar { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationState { get; set; }

        protected PharmacyTransactionsTable sale = new();
        protected DrugStockTable selectedDrug = new();
        protected StockCategoryTable selectedCategory = new();
        protected EmployeesTable pharmacist = new();
        protected PatientsTable Patient = new();
        protected Drugcategory drugcategory = new();
        protected CustomerTypeTable customerType = new();
        protected IList<DrugStockTable> drugs = new List<DrugStockTable>();
        protected IList<EmployeesTable> employees = new List<EmployeesTable>();
        protected IList<StockCategoryTable> _StockCategories = new List<StockCategoryTable>();
        protected IList<AgrovetStockTable> _agrovetStocks = new List<AgrovetStockTable>();
        protected IList<CosmeticsStockTable> _cosmeticsStocks = new List<CosmeticsStockTable>();
        protected IList<Drugcategory> _drugcategories = new List<Drugcategory>();
        protected IList<CustomerTypeTable> _CustomerTypes = new List<CustomerTypeTable>();
        protected IList<PatientsTable> _patients = new List<PatientsTable>();
        protected int? Quantity;
        protected string PatientFirstName = string.Empty;
        protected string PatientLastName = string.Empty;
        protected string PatientCondition = string.Empty;
        protected string DrugName = string.Empty;
        protected string DrugCategoryName = string.Empty;
        protected int DrugCategoryId;
        protected DateTime date = DateTime.Now;
        protected bool notDrug = false;
        protected bool newPatient;
        protected override async Task OnInitializedAsync()
        {
            await GetEmployees();
            await getStockCategories();
            await getDrugCategories();
            await GetCustomerTypes();
            await GetPatients();
        }

        #region Get Functions
        protected async Task GetDrugs()
        {
            var result = await GenericService.GetAllAsync<DrugStockTable>("api/AvanPharmacy/GetDrugs");
            drugs = result.ToList();
        }

        protected async Task GetEmployees()
        {
            var result = await GenericService.GetAllAsync<EmployeesTable>("api/AvanPharmacy/GetAllEmployees");
            employees = result.ToList();
        }

        protected async Task getStockCategories()
        {
            var result = await GenericService.GetAllAsync<StockCategoryTable>("api/AvanPharmacy/GetCategories");
            _StockCategories = result.ToList();
        }

        protected async Task getDrugCategories()
        {
            var result = await GenericService.GetAllAsync<Drugcategory>("api/AvanPharmacy/GetDrugsCategories");
            _drugcategories = result.ToList();
        }

        protected async Task GetCustomerTypes()
        {
            var result = await GenericService.GetAllAsync<CustomerTypeTable>("api/AvanPharmacy/GetCustomerTypes");
            _CustomerTypes = result.ToList();
        }
        protected async Task GetPatients()
        {
            var result = await GenericService.GetAllAsync<PatientsTable>("api/AvanPharmacy/GetPatients");
            _patients = result.ToList();
        }
        #endregion
        protected async Task SubmitSale()
        {
            if (customerType.Id == 1)
            {
                if (selectedDrug.DrugName != null)
                {
                    selectedDrug.Quantity = selectedDrug.Quantity - Quantity;
                    sale.DrugId = selectedDrug.Id;
                    sale.DrugCategoryId = drugcategory.Id;
                    await GenericService.UpdateAsync("api/AvanPharmacy/UpdateDrugs", selectedDrug);
                }

                if (sale.Cosmetic != null)
                {
                    sale.CosmeticId = sale.Cosmetic.Id;
                }
                sale.PrescribedBy = pharmacist.Id;
                sale.StockCategoryId = selectedCategory.Id;
                sale.Date = date;
                sale.Condition = PatientCondition;
                sale.CustomerTypeID = customerType.Id;
                sale.Patient = null;
                sale.Drug = null;
                sale.Cosmetic = null;

                if (sale.DrugId != null)
                {
                    double? DrugAmount = drugs.Where(x => x.Id == sale.DrugId).Select(x => x.Price).FirstOrDefault();
                    sale.saleAmout = Convert.ToDouble(DrugAmount * Quantity);
                }
                else if (sale.CosmeticId != null)
                {
                    double? CosmeticAmount = _cosmeticsStocks.Where(x => x.Id == sale.CosmeticId).Select(x => x.Price).FirstOrDefault();
                    sale.saleAmout = Convert.ToDouble(CosmeticAmount * Quantity);
                }

                sale.Quantity = Quantity;
                await GenericService.SaveAllAsync("api/AvanPharmacy/SaveSale", sale);
                Snackbar.Add("Transaction Added Successfully", Severity.Success);
            }
            else if (customerType.Id == 2)
            {
                if (selectedDrug.DrugName != null)
                {
                    selectedDrug.Quantity = selectedDrug.Quantity - Quantity;
                    sale.DrugId = selectedDrug.Id;
                    sale.DrugCategoryId = drugcategory.Id;
                    await GenericService.UpdateAsync("api/AvanPharmacy/UpdateDrugs", selectedDrug);
                }

                if (sale.Cosmetic != null)
                {
                    sale.CosmeticId = sale.Cosmetic.Id;
                }
                if (newPatient)
                {
                    Patient.FirstName = PatientFirstName;
                    Patient.LastName = PatientLastName;

                    var result = await GenericService.SaveAllAsync("api/AvanPharmacy/SavePatient", Patient);
                    sale.Patient = result;
                }
                else
                {
                    sale.Patient = Patient;
                }
                sale.PatientId = sale.Patient.Id;
                sale.PrescribedBy = pharmacist.Id;
                sale.StockCategoryId = selectedCategory.Id;
                sale.Date = date;
                sale.Condition = PatientCondition;
                sale.CustomerTypeID = customerType.Id;
                sale.Patient = null;
                sale.Drug = null;
                sale.Cosmetic = null;

                if (sale.DrugId != null)
                {
                    double? DrugAmount = drugs.Where(x => x.Id == sale.DrugId).Select(x => x.Price).FirstOrDefault();
                    sale.saleAmout = Convert.ToDouble(DrugAmount * Quantity);
                }
                else if (sale.CosmeticId != null)
                {
                    double? CosmeticAmount = _cosmeticsStocks.Where(x => x.Id == sale.CosmeticId).Select(x => x.Price).FirstOrDefault();
                    sale.saleAmout = Convert.ToDouble(CosmeticAmount * Quantity);
                }

                sale.Quantity = Quantity;
                await GenericService.SaveAllAsync("api/AvanPharmacy/SaveSale", sale);
                Snackbar.Add("Transaction Added Successfully", Severity.Success);
            }
        }

        protected void DrugSale()
        {
            notDrug = true;
        }

        #region String Functions
        protected Func<DrugStockTable, string> Drugsconverter = p => p.DrugName;
        protected Func<EmployeesTable, string> Employeesconverter = p => p.LastName;
        protected Func<StockCategoryTable, string> Categoryconverter = p => p.StockCategoryName;
        protected Func<CustomerTypeTable, string> CustomerTypeconverter = p => p.CustomerTypeDescription;
        protected Func<PatientsTable, string> Patientconverter = p => p.FirstName;
        protected Func<Drugcategory, string> DrugCategoryconverter = p => p.DrugCategoryName;
        protected Func<CosmeticsStockTable, string> Cosmeticconverter = p => p.CosmeticName;
        #endregion
        #region Cascade
        protected async Task OnDrugCateforySelected()
        {
            drugs.Clear();
            DrugName = string.Empty;
            DrugCategoryId = drugcategory.Id;
            DrugCategoryName = _drugcategories.FirstOrDefault(s => s.Id == DrugCategoryId).DrugCategoryName;
            var res = await GenericService.GetAllAsync<DrugStockTable>($"api/AvanPharmacy/GetDrugsByDrugCategory/{DrugCategoryId}");
            drugs = res.ToList();
            StateHasChanged();
        }

        protected async Task GetCosmetics()
        {
            if (selectedCategory.StockCategoryName == "Cosmetics")
            {
                var res = await GenericService.GetAllAsync<CosmeticsStockTable>("api/AvanPharmacy/GetCosmetics");
                _cosmeticsStocks = res.ToList();
            }
        }
    }
    #endregion
}