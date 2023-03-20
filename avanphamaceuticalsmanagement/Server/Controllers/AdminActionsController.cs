using avanphamaceuticalsmanagement.Shared.IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace avanphamaceuticalsmanagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminActionsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminActionsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
         
        }


        [HttpPost("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture(ApplicationUser applicationUser)
        {
            string id = applicationUser.Id;
            string userpic = applicationUser.ProfilePicture;
            var update = await _userManager.FindByIdAsync(id);
            if (update != null)
            {

                update.ProfilePicture = userpic;
                var result = await _userManager.UpdateAsync(update);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
