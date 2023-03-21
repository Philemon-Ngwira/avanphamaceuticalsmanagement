using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using avanphamaceuticalsmanagement.Shared.IdentityModel;
using System.Net.Http.Json;

namespace avanphamaceuticalsmanagement.Client.Pages.Dashboard
{
    public partial class IndexBase : ComponentBase
    {
        protected int LineIndex = -1; //default value cannot be 0 -> first selectedindex is 0.
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        IGenericService _genericService { get; set; }

        TimeSpan time = new TimeSpan();
        protected IList<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        protected IList<PatientsTable> _patients = new List<PatientsTable>();
        protected IList<DrugStockTable> _drugs = new List<DrugStockTable>();
        protected IList<EmployeesTable> employees = new List<EmployeesTable>();
        protected IList<PharmacyTransactionsTable> _sales = new List<PharmacyTransactionsTable>();
        protected IList<StockCategoryTable> stockCategories = new List<StockCategoryTable>();
        public IList<RestockRequestsTable> _restockRequests = new List<RestockRequestsTable>();
        protected IList<BudgetsTable> budgets = new List<BudgetsTable>();
        protected IList<ExpensesTable> _expenses = new List<ExpensesTable>();
        protected int? drugStock = 0;
        protected string SalesError = string.Empty;
        protected double? totalRevenue = 0;
        protected double[] salesData = new double[12];
        protected double[] donutdata;
        protected string[] labels;
        protected List<string> names = new();
        protected string formattedRevenueValue;
        protected string formattedExpenseValue;
        protected string RoleName;
        public int numberofRequests = 0;
        protected MudListItem trajectoryItem = new MudListItem();
        protected bool isBetterThanLastMonth;
        protected double percentage;
        protected string percentageChangeString;
        protected string patienterror = string.Empty;
        protected Dictionary<string, double> categorySales = new Dictionary<string, double>();
        protected double totalSales;
        protected double IncomeBudget;
        protected string totalsalesString = string.Empty;
        protected ChartOptions chartOptions = new ChartOptions();
        public string profilepic = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = auth.User;
            name = user.Identity.Name;
            if (user.IsInRole("Admin"))
            {
                RoleName = "Administrator";
                await GetApprovalRequests();

            }
            else if (user.IsInRole("Employee"))
            {
                RoleName = "Employee";
            }
            await GetAllUsersAsync();
            await GetPPAsync();
            await GetBudgets();
            await GetAllPatients();
            await GetAllEmployees();
            await GetAllDrugs();
            await GetTransactions();
            await getStockCategories();
            await FillChart();
            await FillDonutChart();
            CalculateSalesTrajectory();
            await GetProgressData();

        }

        protected string name = "";
        #region Get Functions

        protected async Task GetBudgets()
        {

            var data = await _genericService.GetAllAsync<BudgetsTable>("api/AvanPharmacy/GetBudgetsDashBoard");
            budgets = data.ToList();
        }
        protected async Task GetAllPatients()
        {
            var result = await _genericService.GetAllAsync<PatientsTable>("api/AvanPharmacy/GetPatients");
            _patients = result.ToList();
            Task.Delay(1);
            if (_patients.Count == 0)
            {
                patienterror = "No Patients Found.";
            }
        }
        protected async Task GetApprovalRequests()
        {
            var requests = await _genericService.GetAllAsync<RestockRequestsTable>("api/AvanPharmacy/GetRestockRequestsDashboard");
            _restockRequests = requests.ToList();
            numberofRequests = _restockRequests.Count;
            StateHasChanged();
        }
        protected async Task GetAllEmployees()
        {
            var result = await _genericService.GetAllAsync<EmployeesTable>("api/AvanPharmacy/GetAllEmployees");
            employees = result.ToList();
        }

        protected async Task GetAllDrugs()
        {

            var result = await _genericService.GetAllAsync<DrugStockTable>("api/AvanPharmacy/GetDrugs");
            _drugs = result.ToList();
            drugStock = _drugs.Select(x => x.Quantity).Sum();
        }
        protected async Task getStockCategories()
        {
            var result = await _genericService.GetAllAsync<StockCategoryTable>("api/AvanPharmacy/GetCategories");
            stockCategories = result.ToList();
        }
        protected async Task GetTransactions()
        {
            var result = await _genericService.GetAllAsync<PharmacyTransactionsTable>("api/AvanPharmacy/GetAllTransactions");
            _sales = result.ToList();
            totalRevenue = _sales.Sum(x => x.saleAmout);
            double value = (double)totalRevenue;
            formattedRevenueValue = value.ToString("#,##0.00");
            Task.Delay(1);
            if (_sales.Count == 0)
            {
                SalesError = "No sales Recorded Yet.";
            }
        }
        protected void CalculateSalesTrajectory()
        {

            // Get the current month and year
            DateTime now = DateTime.Now;
            int currentMonth = now.Month;
            int currentYear = now.Year;

            // Find the sales amount for the current month
            double currentSalesAmount = _sales.Where(s => s.Date.Value.Month == currentMonth && s.Date.Value.Year == currentYear)?.Sum(x => x.saleAmout) ?? 0;

            // Find the sales amount for the previous month
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            int previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;
            double previousSalesAmount = _sales.Where(s => s.Date.Value.Month == previousMonth && s.Date.Value.Year == previousYear)?.Sum(x => x.saleAmout) ?? 0;

            // Calculate the percentage increment or decrease
            double percentageChange = 0;
            if (previousSalesAmount == 0 && currentSalesAmount > 0)
            {
                percentageChange = 100;

            }
            else if (previousSalesAmount != 0)
            {
                percentageChange = (double)(currentSalesAmount - previousSalesAmount) / previousSalesAmount * 100;
            }

            if (percentageChange == 100)
            {
                percentage = 100;
                trajectoryItem.Icon = @Icons.Material.Filled.ArrowUpward;
                trajectoryItem.IconColor = Color.Success;
                trajectoryItem.IconSize = Size.Large;
                percentageChangeString = percentage.ToString("0.00'%'", CultureInfo.InvariantCulture);
            }
            else if (currentSalesAmount < previousSalesAmount)
            {
                percentage = percentageChange;
                trajectoryItem.Icon = @Icons.Material.Filled.TrendingDown;
                trajectoryItem.IconColor = Color.Error;
                trajectoryItem.IconSize = Size.Large;
                percentageChangeString = percentage.ToString("0.00'%'", CultureInfo.InvariantCulture);
            }
            else if (currentSalesAmount > previousSalesAmount && percentageChange != 100)
            {
                percentage = percentageChange;
                trajectoryItem.Icon = @Icons.Material.Filled.TrendingUp;
                trajectoryItem.IconColor = Color.Success;
                trajectoryItem.IconSize = Size.Large;
                percentageChangeString = percentage.ToString("0.00'%'", CultureInfo.InvariantCulture);
            }
        }
        #endregion
        #region Line Chart
        public List<ChartSeries> MonthlySales = new List<ChartSeries>();

        public string[] XAxisLabels =
        {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"
        };

        public ChartSeries MonthlySalesChart = new ChartSeries();
        public ChartSeries MonthlyExpensesChart = new ChartSeries();
        public async Task FillChart()
        {
            MonthlySalesChart.Name = "Monthly Sales";
            double[] monthlysales = new double[12];
            foreach (var item in _sales)
            {
                int month = item.Date.Value.Month;

                monthlysales[month - 1] += item.saleAmout;

            }
            salesData = monthlysales;
            MonthlySalesChart.Data = salesData;


            MonthlyExpensesChart.Name = "Monthly Expenses";

            double[] monthlyExpenses = new double[12];

            // Assign expenses to the corresponding month in the array
            var result = await _genericService.GetAllAsync<ExpensesTable>("api/AvanPharmacy/GetExpenses");
            _expenses = result.ToList();
            foreach (var expense in _expenses)
            {
                int month = expense.Date.Value.Month;
                monthlyExpenses[month - 1] += (double)expense.Amount;
            }
            MonthlyExpensesChart.Data = monthlyExpenses;
            MonthlySales.Add(MonthlySalesChart);
            MonthlySales.Add(MonthlyExpensesChart);

            chartOptions.YAxisLines = true;
            chartOptions.InterpolationOption = InterpolationOption.Straight;
            double value = (double)_expenses.Select(x => x.Amount).Sum();
            formattedExpenseValue = value.ToString("#,##0.00");
        }
        #endregion
        #region Donught Chart
        protected async Task FillDonutChart()
        {
            foreach (var item in _sales)
            {

                item.StockCategory = stockCategories.Where(x => x.Id == item.StockCategoryId).FirstOrDefault();
                if (categorySales.ContainsKey(item.StockCategory.StockCategoryName))
                {
                    categorySales[item.StockCategory.StockCategoryName] += item.saleAmout;
                }
                else
                {
                    categorySales.Add(item.StockCategory.StockCategoryName, item.saleAmout);
                }
                //item.StockCategory = stockCategories.Where(x => x.Id == item.StockCategoryId).FirstOrDefault();
            }
            labels = categorySales.Keys.ToArray();
            names = labels.ToList();
            donutdata = categorySales.Values.ToArray();
        }
        #endregion

        #region ProgressBars
        protected async Task GetProgressData()
        {
            int currentYear = DateTime.Now.Year;
            string IncomeBudgetName = "Operating Income";
            IncomeBudget = (double)budgets.Where(x => x.BudgetType.BudgetType == IncomeBudgetName && x.StartDate.Value.Year == currentYear).Select(x => x.Amount).Sum();
            var num = _sales.Where(x => x.Date.Value.Year == currentYear).Select(X => X.saleAmout).Sum();
            totalSales = num / IncomeBudget * 100;
            totalsalesString = totalSales.ToString("0.00'%'", CultureInfo.InvariantCulture);
        }
        #endregion

        protected async Task GetPPAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = auth.User;

            foreach (var item in Users)
            {
                if (item.UserName.Contains(user.Identity.Name))
                {
                    profilepic = item.ProfilePicture;
                }
            }



        }

        protected async Task GetAllUsersAsync()
        {
            if (Users.Count == 0)
            {

                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetFromJsonAsync<List<ApplicationUser>>("https://localhost:7131/api/Admin");
                    Users = response;



                }
                catch (Exception ex)
                {
                    var _ = ex.Message;
                    throw;
                }
            }



        }
    }
}
