using avanphamaceuticalsmanagement.Shared.Models;
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

    }
}
