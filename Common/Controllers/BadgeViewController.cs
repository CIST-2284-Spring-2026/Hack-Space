using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeViewController : ControllerBase
    {
        private readonly IBadgeViewDAL dal;
 
        public BadgeViewController(IBadgeViewDAL dal)
        {
            this.dal = dal;
        }
 
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var badge = await this.dal.GetByIdAsync(id);
            if (badge == null)
            {
                return NotFound();
            }
 
            return Ok(badge);
        }
    }
}