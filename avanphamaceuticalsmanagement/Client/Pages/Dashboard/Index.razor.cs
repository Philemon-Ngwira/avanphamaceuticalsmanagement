using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;

namespace avanphamaceuticalsmanagement.Client.Pages.Dashboard
{
    public partial class Index
    {
        private int LineIndex = -1; //default value cannot be 0 -> first selectedindex is 0.
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        IGenericService _genericService { get; set; }

        TimeSpan time = new TimeSpan();
        protected IList<PatientsTable> _patients = new List<PatientsTable>();
        protected IList<DrugStockTable> _drugs = new List<DrugStockTable>();
        protected IList<EmployeesTable> employees = new List<EmployeesTable>();
        protected IList<PharmacyTransactionsTable> _sales = new List<PharmacyTransactionsTable>();
        protected int? drugStock = 0;
        protected double? totalRevenue = 0;
        protected double[] salesData = new double[12];
        string formattedRevenueValue;
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = auth.User;
            name = user.Identity.Name;
            await GetAllPatients();
            await GetAllEmployees();
            await GetAllDrugs();
            await GetTransactions();
            await FillChart();
        }

        protected string name = "Philemon";
        #region Get Functions
        protected async Task GetAllPatients()
        {
            var result = await _genericService.GetAllAsync<PatientsTable>("api/AvanPharmacy/GetPatients");
            _patients = result.ToList();
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

        protected async Task GetTransactions()
        {
            var result = await _genericService.GetAllAsync<PharmacyTransactionsTable>("api/AvanPharmacy/GetTransactions");
            _sales = result.ToList();
            totalRevenue = _sales.Sum(x => x.saleAmout);
             double value = (double)totalRevenue;
            formattedRevenueValue = value.ToString("#,##0.00");
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
            double Jan =Convert.ToDouble( _sales.Where(x => x.Date.Value.Month == 1).Sum(x => x.saleAmout));
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
    }
}
