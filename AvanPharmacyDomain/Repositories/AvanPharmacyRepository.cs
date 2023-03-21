using avanphamaceuticalsmanagement.Shared.Models;
//using test.Shared.Models;
using AvanPharmacyDomain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvanPharmacyDomain.Repositories
{
    public class AvanPharmacyRepository : GenericRepository
    {
        private readonly avanpharmacyDbContext _dbContext;

        public AvanPharmacyRepository(avanpharmacyDbContext dbContext): base (dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PharmacyTransactionsTable>> GetTransactionsTablesAsync()
        {
            try
            {
                var res = await _dbContext.PharmacyTransactionsTables
                    .Include(x=>x.Drug)
                    .Include(x=>x.Patient)
                    .Include(x=>x.PrescribedByNavigation)
                    .ToListAsync();
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<BudgetsTable>> GetBudgetsDashBoard()
        {
            try
            {
                var res = await _dbContext.BudgetsTables
                    .Include(x => x.BudgetType)
                    .ToListAsync ();

                return res;
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }
        }
        public async Task<IEnumerable<RestockRequestsTable>> GetRestockRequests()
        {
            try
            {
                var result = await _dbContext.RestockRequestsTables
                    .Include(x=>x.RequestCategory)
                    .Include(x=>x.RequestCosmetic)
                    .Include(x=>x.RequestDrugCategory)
                    .Include(x=>x.RequestDrug)
                    .Include(x=>x.StatusNavigation)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }
        }
        public IEnumerable<DrugStockTable> GetDrugsByDCIdAsync(int DCid)

        {
            try
            {
                List<DrugStockTable> areaList = new List<DrugStockTable>();
                areaList = (from DrugName in _dbContext.DrugStockTables where DrugName.DrugCat == DCid select DrugName).ToList();
                return areaList;
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }
        }


    }
}
