using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;
using System.Globalization;

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
        protected IList<PatientsTable> _patients = new List<PatientsTable>();
        protected IList<DrugStockTable> _drugs = new List<DrugStockTable>();
        protected IList<EmployeesTable> employees = new List<EmployeesTable>();
        protected IList<PharmacyTransactionsTable> _sales = new List<PharmacyTransactionsTable>();
        protected IList<StockCategoryTable> stockCategories = new List<StockCategoryTable>();
        public IList<RestockRequestsTable> _restockRequests = new List<RestockRequestsTable>();
        protected int? drugStock = 0;
        protected string SalesError = string.Empty;
        protected double? totalRevenue = 0;
        protected double[] salesData = new double[12];
        protected double[] donutdata;
        protected string[] labels;
        protected List<string> names = new();
        protected string formattedRevenueValue;
        protected string RoleName;
        public int numberofRequests = 0;
        protected MudListItem trajectoryItem = new MudListItem();
        protected bool isBetterThanLastMonth;
        protected double percentage;
        protected string percentageChangeString;
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
            await GetAllPatients();
            await GetAllEmployees();
            await GetAllDrugs();
            await GetTransactions();
            await getStockCategories();
            await FillChart();
            await FillDonutChart();
            await CalculateSalesTrajectory();

        }

        protected string name = "Philemon";
        #region Get Functions
        protected async Task GetAllPatients()
        {
            var result = await _genericService.GetAllAsync<PatientsTable>("api/AvanPharmacy/GetPatients");
            _patients = result.ToList();
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
        protected async Task CalculateSalesTrajectory()
        {

            // Get the current month and year
            DateTime now = DateTime.Now;
            int currentMonth = now.Month;
            int currentYear = now.Year;

            // Find the sales amount for the current month
            double currentSalesAmount = _sales.FirstOrDefault(s => s.Date.Value.Month == currentMonth && s.Date.Value.Year == currentYear)?.saleAmout ?? 0;

            // Find the sales amount for the previous month
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            int previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;
            double previousSalesAmount = _sales.FirstOrDefault(s => s.Date.Value.Month == previousMonth && s.Date.Value.Year == previousYear)?.saleAmout ?? 0;

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
                percentageChangeString=percentage.ToString("0.00'%'", CultureInfo.InvariantCulture);
            }
            else if(currentSalesAmount < previousSalesAmount)
            {
                percentage = percentageChange;
                trajectoryItem.Icon = @Icons.Material.Filled.TrendingDown;
                trajectoryItem.IconColor = Color.Error;
                percentageChangeString = percentage.ToString("0.00'%'", CultureInfo.InvariantCulture);
            }
            else if(currentSalesAmount > previousSalesAmount && percentageChange != 100)
            {
                percentage = percentageChange;
                trajectoryItem.Icon = @Icons.Material.Filled.TrendingUp;
                trajectoryItem.IconColor = Color.Success;
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
        public async Task FillChart()
        {
            MonthlySalesChart.Name = "Monthly Sales";
            double Jan = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 1).Sum(x => x.saleAmout));
            double Feb = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 2).Sum(x => x.saleAmout));
            double Mar = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 3).Sum(x => x.saleAmout));
            double Apr = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 4).Sum(x => x.saleAmout));
            double May = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 5).Sum(x => x.saleAmout));
            double Jun = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 6).Sum(x => x.saleAmout));
            double Jul = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 7).Sum(x => x.saleAmout));
            double Aug = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 8).Sum(x => x.saleAmout));
            double Sep = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 9).Sum(x => x.saleAmout));
            double Oct = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 10).Sum(x => x.saleAmout));
            double Nov = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 11).Sum(x => x.saleAmout));
            double Dec = Convert.ToDouble(_sales.Where(x => x.Date.Value.Month == 12).Sum(x => x.saleAmout));
            salesData[0] = Jan;
            salesData[1] = Feb;
            salesData[2] = Mar;
            salesData[3] = Apr;
            salesData[4] = May;
            salesData[5] = Jun;
            salesData[6] = Jul;
            salesData[7] = Aug;
            salesData[8] = Sep;
            salesData[9] = Oct;
            salesData[10] = Nov;
            salesData[11] = Dec;
            MonthlySalesChart.Data = salesData;
            MonthlySales.Add(MonthlySalesChart);
        }
        #endregion
        #region Donught Chart
        protected async Task FillDonutChart()
        {
            donutdata = new double[_sales.Count];
            labels = new string[_sales.Count];

            foreach (var item in _sales)
            {
                item.StockCategory = stockCategories.Where(x => x.Id == item.StockCategoryId).FirstOrDefault();
            }
            for (int i = 0; i < _sales.Count; i++)
            {
                PharmacyTransactionsTable sale = _sales[i];

                names.Add(sale.StockCategory.StockCategoryName);
                labels = names.ToArray();
                donutdata[i] = sale.saleAmout;
            }


        }



        #endregion
    }
}
