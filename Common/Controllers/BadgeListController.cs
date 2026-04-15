using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeListController : ControllerBase
    {
        private readonly IBadgeListDAL dal;
 
        public BadgeListController(IBadgeListDAL dal)
        {
            this.dal = dal;
        }
 
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var badges = await this.dal.GetAllAsync();
            return Ok(badges);
        }
 
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Editors")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await this.dal.DeleteAsync(id);
            return Ok();
        }
    }
}