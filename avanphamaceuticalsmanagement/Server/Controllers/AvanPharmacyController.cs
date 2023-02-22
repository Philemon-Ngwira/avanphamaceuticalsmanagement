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

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            var employees = await AvanPharmacyRepository.GetAsync<EmployeesTable>();
            return Ok(employees);
        }
        
        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatientsAsync()
        {
            var employees = await AvanPharmacyRepository.GetAsync<PatientsTable>();
            return Ok(employees);
        }
        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactionsAsync()
        {
            var employees = await AvanPharmacyRepository.GetAsync<PharmacyTransactionsTable>();
            return Ok(employees);
        }

        #endregion

        #region Post
        [HttpPost("PostDrugs")]
        public async Task<IActionResult> SaveDrugs(DrugStockTable drugStock)
        {
            var result = await AvanPharmacyRepository.SaveAllAsync(drugStock);
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
            var result =  await AvanPharmacyRepository.updateAsync(drugStock);
            return Ok(result);
        }
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
