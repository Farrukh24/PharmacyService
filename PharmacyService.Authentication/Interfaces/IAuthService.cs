using Microsoft.AspNetCore.Identity;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Models;

namespace PharmacyService.Authentication
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDTO registerDto);
        Task<SignInResult> LoginUserAsync(LoginDTO loginDto);
        Task<IdentityResult> CreateUserAsync(CreateUserDTO createUserDto);
        Task InitializeRolesAsync();
        Task InitializeUsersAsync(UserManager<ApplicationUser> userManager);
        Task LogoutAsync();
    }
}
