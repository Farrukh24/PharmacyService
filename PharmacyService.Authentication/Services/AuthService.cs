using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Models;
using System;

namespace PharmacyService.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ILogger<AuthService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO registerUserDto)
        {
            try
            {
                _logger.LogInformation("Registering user: {Email}", registerUserDto.Email);

                var user = new ApplicationUser
                {
                    UserName = registerUserDto.Email,
                    Email = registerUserDto.Email
                };

                var result = await _userManager.CreateAsync(user, registerUserDto.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully: {Email}", registerUserDto.Email);
                    await _userManager.AddToRoleAsync(user, "Salesman");
                }
                else
                {
                    _logger.LogWarning("Failed to register user: {Email}", registerUserDto.Email);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user: {Email}", registerUserDto.Email);
                throw;
            }
        }

        public async Task<SignInResult> LoginUserAsync(LoginDTO loginDto)
        {
            try
            {
                _logger.LogInformation("Logging in user: {Email}", loginDto.Email);

                var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
                }
                else
                {
                    _logger.LogWarning("Failed to log in user: {Email}", loginDto.Email);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user: {Email}", loginDto.Email);
                throw;
            }
        }

        public async Task InitializeRolesAsync()
        {
            try
            {
                _logger.LogInformation("Initializing roles...");

                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    _logger.LogInformation("Admin role created.");
                }
                if (!await _roleManager.RoleExistsAsync("Salesman"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Salesman"));
                    _logger.LogInformation("Salesman role created.");
                }

                _logger.LogInformation("Roles initialized successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while initializing roles.");
                throw;
            }
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserDTO createUserDto)
        {
            try
            {
                _logger.LogInformation("Creating user: {Email}", createUserDto.Email);

                var user = new ApplicationUser
                {
                    UserName = createUserDto.Email,
                    Email = createUserDto.Email
                };

                var result = await _userManager.CreateAsync(user, createUserDto.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created successfully: {Email}", createUserDto.Email);
                    await _userManager.AddToRoleAsync(user, "Salesman");
                }
                else
                {
                    _logger.LogWarning("Failed to create user: {Email}", createUserDto.Email);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user: {Email}", createUserDto.Email);
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                _logger.LogInformation("Logging out user...");

                await _signInManager.SignOutAsync();

                _logger.LogInformation("User logged out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging out user.");
                throw;
            }
        }

        public async Task InitializeUsersAsync(UserManager<ApplicationUser> userManager)
        {
            try
            {
                _logger.LogInformation("Initializing users...");

                if (!userManager.Users.Any())
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                    };

                    var salesmanUser = new ApplicationUser
                    {
                        UserName = "salesman@example.com",
                        Email = "salesman@example.com",
                    };

                    string adminPassword = "Admin@123";
                    string salesmanPassword = "Salesman@123";

                    await userManager.CreateAsync(adminUser, adminPassword);
                    await userManager.CreateAsync(salesmanUser, salesmanPassword);

                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddToRoleAsync(salesmanUser, "Salesman");
                }

                _logger.LogInformation("Users initialized successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while initializing users.");
                throw;
            }
        }

    }
}
