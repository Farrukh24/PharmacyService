using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Authentication;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace PharmacyService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                _logger.LogInformation("Attempting to register user: {Email}", registerDto.Email);

                var result = await _authService.RegisterUserAsync(registerDto);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
                    return Ok("User registered successfully.");
                }
                else
                {
                    _logger.LogWarning("Failed to register user: {Email}", registerDto.Email);
                    return BadRequest("Failed to register user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration: {Email}", registerDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                _logger.LogInformation("Attempting to log in user: {Email}", loginDto.Email);

                var result = await _authService.LoginUserAsync(loginDto);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
                    return Ok("Login successful.");
                }
                else
                {
                    _logger.LogWarning("Failed to log in user: {Email}", loginDto.Email);
                    return Unauthorized("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user login: {Email}", loginDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                _logger.LogInformation("Logging out user...");

                await _authService.LogoutAsync();

                _logger.LogInformation("User logged out successfully.");
                return Ok("Logout successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user logout.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create user: {Email}", createUserDto.Email);

                var result = await _authService.CreateUserAsync(createUserDto);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created successfully: {Email}", createUserDto.Email);
                    return Ok("User created successfully.");
                }
                else
                {
                    _logger.LogWarning("Failed to create user: {Email}", createUserDto.Email);
                    return BadRequest("Failed to create user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user creation: {Email}", createUserDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
