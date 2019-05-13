namespace IPCViewer.Api.Helpers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserHelper (
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync (User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync (User user, string roleName)
        {
            await userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync (User user, string oldPassword, string newPassword)
        {
            return await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync (string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if ( !roleExists )
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserByEmailAsync (string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsUserInRoleAsync (User user, string roleName)
        {
            return await userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync (LoginViewModel model)
        {
            return await signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync ()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync (User user)
        {
            return await userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync (User user, string password)
        {
            return await signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        //public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        //{
        //    return await userManager.ConfirmEmailAsync(user, token);
        //}

        //public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        //{
        //    return await userManager.GenerateEmailConfirmationTokenAsync(user);
        //}

        public async Task<User> GetUserByIdAsync (string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync (User user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync (User user, string token, string password)
        {
            return await userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<List<User>> GetAllUsersAsync ()
        {
            var users = await userManager.Users.Include(u => u.City).ToListAsync();
            return users;
        }

        public async Task RemoveUserFromRoleAsync (User user, string roleName)
        {
            await userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task DeleteUserAsync (User user)
        {
            await userManager.DeleteAsync(user);
        }
    }
}