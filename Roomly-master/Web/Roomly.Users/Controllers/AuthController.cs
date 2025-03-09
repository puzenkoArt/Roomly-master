using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Roomly.Shared.Auth.Services;
using Roomly.Shared.Data.Enums;
using Roomly.Users.Infrastructure.Exceptions;
using Roomly.Users.ViewModels;
using StackExchange.Redis;

namespace Roomly.Users.Controllers;

[ApiController]
[Route("api/users")]
public class AuthController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IdentityService _identityService;
    
    public AuthController(
        IHttpContextAccessor contextAccessor,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IdentityService identityService)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _identityService = identityService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user is null)
                return BadRequest(new EntityNotFoundException().Message);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);
            if (!result.Succeeded)
                return BadRequest("Could not sign in");

            var claims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email),
            });

            claimsIdentity.AddClaims(claims);

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = _identityService.WriteToken(token);

            _contextAccessor.HttpContext.Response.Cookies.Append("token", response, new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            
            Response.Headers.Add("Authorization", "Bearer " + response);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel registrationViewModel)
    {
        try
        {
            var roleExists = await _roleManager.RoleExistsAsync("Administrator");
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            }
            
            var identity = new IdentityUser()
            {
                Email = registrationViewModel.Email,
                UserName = registrationViewModel.Name
            };

            var userExists = await _userManager.FindByEmailAsync(identity.Email);
            if (userExists is not null)
            {
                return BadRequest("User already exists");
            }
            
            var result = await _userManager.CreateAsync(identity, registrationViewModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            var newClaims = new List<Claim>
            {
                new Claim("Name", registrationViewModel.Name),
                new Claim("Email", registrationViewModel.Email),
            };
            
            await _userManager.AddClaimsAsync(identity, newClaims);
            await _userManager.AddToRoleAsync(identity, Roles.Administrator.ToString());
            
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete("token", new CookieOptions
            {
                MaxAge = TimeSpan.FromSeconds(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}