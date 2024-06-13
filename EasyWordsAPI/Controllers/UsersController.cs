using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EasyWordsAPI.Models;
using EasyWordsAPI.Models.DataTransfer;
using EasyWordsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EasyWordsAPI.Controllers;



[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private UserManager<IdentityUser<int>> users;
    private readonly SignInManager<IdentityUser<int>> signIn;

    public UsersController(UserManager<IdentityUser<int>> users,
        SignInManager<IdentityUser<int>> signIn)
    {
        this.users = users;
        this.signIn = signIn;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var user = await users.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            return NotFound("Неверное имя пользователя или пароль");
        }

        if (!await users.CheckPasswordAsync(user, dto.Password))
        {
            return NotFound("Неверное имя пользователя или пароль");
        }
        
        var principal = await signIn.CreateUserPrincipalAsync(user);

        return Ok(TokenService.GetToken(principal));
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Registration(RegistrationDTO dto)
    {
        var user = new IdentityUser<int>
        {
            UserName = dto.Username,
            Email = dto.Email
        };

        var result = await users.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        result = await users.AddToRoleAsync(user, "client");

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Success");
    }
}