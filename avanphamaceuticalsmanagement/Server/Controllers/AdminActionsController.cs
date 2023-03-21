using avanphamaceuticalsmanagement.Server.Data;
using avanphamaceuticalsmanagement.Shared.IdentityModel;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AdminContext _context;
        
        public AdminActionsController(UserManager<ApplicationUser> userManager, AdminContext context)
        {
            _userManager = userManager;
            _context = context;

        }

        [HttpPost("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] ApplicationUser applicationUser)
        {
            string id = applicationUser.Id;
            string userpic = applicationUser.ProfilePicture;
            var update = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (update != null)
            {
                    update.ProfilePicture = userpic;
                    _context.Users.Update(update);
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
