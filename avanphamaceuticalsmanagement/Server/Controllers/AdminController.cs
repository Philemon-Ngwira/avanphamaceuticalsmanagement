using avanphamaceuticalsmanagement.Server.Models;

using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using avanphamaceuticalsmanagement.Client.Pages.Administration;


namespace avanphamaceuticalsmanagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        //{
        //    try
        //    {
        //        var users =  _userManager.Users.AsAsyncEnumerable();
        //        return Ok(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        var _ = new Exception(ex.Message);
        //        throw;
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllUsers()
        {
            try
            {
                var users = _userManager.Users.AsEnumerable().ToList();
                
                var result = new List<UserViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                  
                    var userViewModel = new UserViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        Roles = roles
                    };
                    result.Add(userViewModel);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                var _ = new Exception(ex.Message);
                throw;
            }
        }
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).AsEnumerable().ToList();
            return Ok(roles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }


        [HttpPost("SaveAllRoles")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (roleExist)
                {
                    return BadRequest("Role already exists");
                }

                var role = new IdentityRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Ok(role);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}/roles")]
        public async Task<IActionResult> UpdateUserRoles(string id, [FromBody] string[] roleNames)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = roleNames.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(roleNames);

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return NoContent();
        }

        
    }
}
