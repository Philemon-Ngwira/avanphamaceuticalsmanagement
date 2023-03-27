using avanphamaceuticalsmanagement.Server.Data;
using avanphamaceuticalsmanagement.Shared.IdentityModel;
using avanphamaceuticalsmanagement.Shared.Models;
using AvanPharmacyDomain.Data;
using AvanPharmacyDomain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace avanphamaceuticalsmanagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminActionsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly AvanPharmacyRepository _avatarRepository;
        private readonly avanpharmacyDbContext _avatarDbContext;
        public AdminActionsController(ApplicationDbContext context, AvanPharmacyRepository repository, avanpharmacyDbContext
             dbContext)
        {
            _avatarRepository = repository;
            _context = context;
            _avatarDbContext = dbContext;

        }

        [HttpPost("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] ApplicationUser applicationUser)
        {
            AspNetUser user = new();
            string id = applicationUser.Id;
            user = _avatarDbContext.AspNetUsers.Where(x=>x.Id == id).FirstOrDefault();
            string userpic = applicationUser.ProfilePicture;
            var update = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (update != null)
            {
                update.ProfilePicture = userpic;
                _context.Update(update);
                await _avatarRepository.updateAsync(update);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
