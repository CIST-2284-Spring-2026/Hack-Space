using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeEditController : ControllerBase
    {
        private readonly IBadgeEditDAL dal;
 
        public BadgeEditController(IBadgeEditDAL dal)
        {
            this.dal = dal;
        }
 
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var badgeEditDto = await this.dal.GetBadgeEditDtoAsync(id);
            if (badgeEditDto == null)
            {
                return NotFound();
            }
 
            return Ok(badgeEditDto);
        }
 
        [HttpPost]
        [Authorize(Roles = "Admin, Editors")]
        public async Task<IActionResult> AddAsync([FromBody] Common.Models.Badge badge)
        {
            await this.dal.AddAsync(badge);
            return Ok();
        }
 
        [HttpPut]
        [Authorize(Roles = "Admin, Editors")]
        public async Task<IActionResult> UpdateAsync([FromBody] Common.Models.Badge badge)
        {
            await this.dal.UpdateAsync(badge);
            return Ok();
        }
    }
}