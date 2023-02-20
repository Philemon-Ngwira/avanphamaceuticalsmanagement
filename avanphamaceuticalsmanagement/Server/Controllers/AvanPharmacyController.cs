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
        #endregion

        #region Post
        [HttpPost("PostDrugs")]
        public async Task<IActionResult> SaveDrugs(DrugStockTable drugStock)
        {
            var result = await AvanPharmacyRepository.SaveAllAsync(drugStock);
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
    }
}
