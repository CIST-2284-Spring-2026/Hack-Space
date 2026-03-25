using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Identity;
 
namespace Server.Data.DALs
{
    public class UsersDAL : IUsersDAL
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public UsersDAL(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
 
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            List<UserDto> userDtos = new List<UserDto>();
            foreach (ApplicationUser appUser in userManager.Users)
            {
                UserDto userDto = new()
                {
                    Id = appUser.Id,
                    Email = appUser.Email ?? string.Empty,
                    EmailConfirmed = appUser.EmailConfirmed,
                    Password = string.Empty, // Do not return the password
                    RoleSelections = roleManager.Roles.Select(role => new RoleSelection
                    {
                        RoleName = role.Name ?? string.Empty,
                        IsSelected = userManager.IsInRoleAsync(appUser, role.Name ?? string.Empty).Result
                    }).ToList()
                };
                userDtos.Add(userDto);
            }
            return userDtos;
        }
 
        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            ApplicationUser? appUser = await userManager.FindByIdAsync(userId);
            if (appUser == null)
            {
                return null;
            }
 
            var userDto = new UserDto
            {
                Email = appUser.Email ?? string.Empty,
                EmailConfirmed = appUser.EmailConfirmed,
                Password = string.Empty, // Do not return the password
                RoleSelections = roleManager.Roles.Select(role => new RoleSelection
                {
                    RoleName = role.Name ?? string.Empty,
                    IsSelected = userManager.IsInRoleAsync(appUser, role.Name ?? string.Empty).Result
                }).ToList()
            };
            return userDto;
        }
 
        public async Task CreateUserAsync(UserDto userDto)
        {
            ApplicationUser appUser = new()
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                EmailConfirmed = userDto.EmailConfirmed
            };
 
            var result = await userManager.CreateAsync(appUser, userDto.Password);
            if (result.Succeeded)
            {
                List<string> rolesToAdd = userDto.RoleSelections.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();
 
                if (rolesToAdd.Any())
                {
                    await userManager.AddToRolesAsync(appUser, rolesToAdd);
                }
            }
        }
 
        public async Task UpdateUserAsync(UserDto userDto)
        {
            ApplicationUser? appUser = await userManager.FindByIdAsync(userDto.Id);
            if (appUser == null)
            {
                return;
            }
 
            appUser.Email = userDto.Email;
            appUser.UserName = userDto.Email;
            appUser.EmailConfirmed = userDto.EmailConfirmed;
            IdentityResult result = await userManager.UpdateAsync(appUser);
 
            // If password is not empty, update it
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                string? token = await userManager.GeneratePasswordResetTokenAsync(appUser);
                var passwordResult = await userManager.ResetPasswordAsync(appUser, token, userDto.Password);
            }
 
            // Update roles
            IList<string> currentRoles = await userManager.GetRolesAsync(appUser);
            List<string> rolesToAdd = userDto.RoleSelections.Where(r => r.IsSelected).Select(r => r.RoleName).Except(currentRoles).ToList();
            List<string> rolesToRemove = currentRoles.Except(userDto.RoleSelections.Where(r => r.IsSelected).Select(r => r.RoleName)).ToList();
 
            if (rolesToAdd.Any())
            {
                await userManager.AddToRolesAsync(appUser, rolesToAdd);
            }
 
            if (rolesToRemove.Any())
            {
                await userManager.RemoveFromRolesAsync(appUser, rolesToRemove);
            }
        }
 
        public async Task DeleteUserAsync(string userId)
        {
            ApplicationUser? appUser = await userManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                await userManager.DeleteAsync(appUser);
            }
        }
    }
}