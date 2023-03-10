using avanphamaceuticalsmanagement.Shared.Models;
using AvanPharmacyDomain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace avanphamaceuticalsmanagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvanPharmacyController : ControllerBase
    {
        private readonly AvanPharmacyRepository AvanPharmacyRepository;

        public AvanPharmacyController(AvanPharmacyRepository repository)
        {
            AvanPharmacyRepository = repository;
        }

        #region Gets
        [HttpGet("GetDrugs")]
        public async Task<IActionResult> GetDrugsAsync()
        {
            var drugs = await AvanPharmacyRepository.GetAsync<DrugStockTable>();
            return Ok(drugs);
        }
        [HttpGet("GetBudgetsDashBoard")]
        public async Task<IActionResult> GetBudgetsDashBoardAsync()
        {
            var drugs = await AvanPharmacyRepository.GetBudgetsDashBoard();
            return Ok(drugs);
        }


        [HttpGet("GetDrugsCategories")]
        public async Task<IActionResult> GetDrugCatAsync()
        {
            var drugs = await AvanPharmacyRepository.GetAsync<Drugcategory>();
            return Ok(drugs);
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            var employees = await AvanPharmacyRepository.GetAsync<EmployeesTable>();
            return Ok(employees);
        }

        [HttpGet("GetBudgetTypes")]
        public async Task<IActionResult> GetBudgetTypes()
        {
            var employees = await AvanPharmacyRepository.GetAsync<BudgetTypeTable>();
            return Ok(employees);
        }

        [HttpGet("GetBudgets")]
        public async Task<IActionResult> GetBudgets()
        {
            var budgets = await AvanPharmacyRepository.GetAsync<BudgetsTable>();
            return Ok(budgets);
        }

        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatientsAsync()
        {
            var employees = await AvanPharmacyRepository.GetAsync<PatientsTable>();
            return Ok(employees);
        }
        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetTransactionsAsync()
        {
            var employees = await AvanPharmacyRepository.GetAsync<PharmacyTransactionsTable>();
            return Ok(employees);
        }

        [HttpGet("GetCustomertypes")]
        public async Task<IActionResult> GetCustomerTypes()
        {
            var result = await AvanPharmacyRepository.GetAsync<CustomerTypeTable>();
            return Ok(result);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var categoryTables = await AvanPharmacyRepository.GetAsync<StockCategoryTable>();
            return Ok(categoryTables);
        }
        [HttpGet("GetCosmetics")]
        public async Task<IActionResult> GetCosmetics()
        {
            var cosmetic = await AvanPharmacyRepository.GetAsync<CosmeticsStockTable>();
            return Ok(cosmetic);
        }
        [HttpGet("GetRestockRequests")]
        public async Task<IActionResult> GetRestockRequest()
        {
            var request = await AvanPharmacyRepository.GetRestockRequests();
            return Ok(request);
        }
        [HttpGet("GetRestockRequestsDashboard")]
        public async Task<IActionResult> GetRequestCount()
        {
            var cosmetic = await AvanPharmacyRepository.GetAsync<RestockRequestsTable>();
            return Ok(cosmetic);
        }
        //Get By ID 

        [HttpGet]
        [Route("GetDrugsByDrugCategory/{id}")]

        public IEnumerable<DrugStockTable> GetDrugs(int id)
        {
            var res = AvanPharmacyRepository.GetDrugsByDCIdAsync(id);

            return res;
        }
        #endregion

        #region Post
        [HttpPost("PostDrugs")]
        public async Task<IActionResult> SaveDrugs(DrugStockTable drugStock)
        {
            var result = await AvanPharmacyRepository.SaveAllAsync(drugStock);
            return Ok(result);
        }

        [HttpPost("SaveBudget")]
        public async Task<IActionResult> SaveBudget(BudgetsTable Budget)
        {
            var result = await AvanPharmacyRepository.SaveAllAsync(Budget);
            return Ok(result);
        }

        [HttpPost("SaveRestockRequest")]
        public async Task<IActionResult> SaveRestockRequest(RestockRequestsTable Stock)
        {
            var result = await AvanPharmacyRepository.SaveAllAsync(Stock);
            return Ok(result);
        }
        [HttpPost("SavePatient")]
        public async Task<IActionResult> SavePatient(PatientsTable patient)
        {
            var result = await AvanPharmacyRepository.SaveAllAsync(patient);
            return Ok(result);
        }
        [HttpPost("SaveSale")]
        public async Task<IActionResult> SaveSale(PharmacyTransactionsTable sale)
        {
            var result = await AvanPharmacyRepository.SaveAllAsync(sale);
            return Ok(result);
        }

        #endregion

        #region Update
        [HttpPut("UpdateDrugs")]
        public async Task<IActionResult> UpdateDrugs(DrugStockTable drugStock)
        {
            var result = await AvanPharmacyRepository.updateAsync(drugStock);
            return Ok(result);
        }
        [HttpPut("UpdateRestockRequest")]
        public async Task<IActionResult> UpdateRestockRequest(RestockRequestsTable drugStock)
        {
            var result = await AvanPharmacyRepository.updateAsync(drugStock);
            return Ok(result);
        }
        //
        #endregion

        #region Delete
        [HttpDelete("DeleteDrugs/{Id}")]
        public async Task<IActionResult> DeleteDrugs(int id)
        {
            await AvanPharmacyRepository.DeleteAllAsync<DrugStockTable>(id);
            return NoContent();
        }
        #endregion
    }
}
