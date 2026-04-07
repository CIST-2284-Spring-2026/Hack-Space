// Server/Controllers/BadgesController.cs

using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.DALs;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgesController(IBadgesDAL1 dal) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var badges = await dal.GetBadgesAsync();
            return Ok(badges);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var badge = await dal.GetBadgeByIdAsync(id);
            return badge == null ? NotFound() : Ok(badge);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editors")]
        public async Task<IActionResult> AddAsync([FromBody] Badge badge)
        {
            if (badge == null) return BadRequest("Badge is required");

            await dal.AddBadgeAsync(badge);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Editors")]
        public async Task<IActionResult> UpdateAsync([FromBody] Badge badge)
        {
            if (badge == null || badge.Id == Guid.Empty)
                return BadRequest("Valid badge with Id is required");

            await dal.UpdateAsync(badge);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Editors")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await dal.DeleteAsync(id);
            return Ok();
        }
    }

    public interface IBadgesDAL1
    {
        public Task<List<Badge>?> GetBadgesAsync();
        public Task<Badge?> GetBadgeByIdAsync(Guid id);
        public Task AddBadgeAsync(Badge badge);
        public Task UpdateBadgeAsync(Badge badge);
        public Task DeleteAsync(Badge badge);

        public Task UpdateAsync(Badge badge);
        Task GetAllAsync();
        Task GetAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}