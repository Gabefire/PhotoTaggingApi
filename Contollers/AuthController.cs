using PhotoTaggingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using PhotoTaggingApi.Services;

namespace PhotoTaggingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public static User user = new User();
    private readonly IConfiguration _configuration;
    private readonly UsersService _usersService;
    public AuthController(IConfiguration configuration, UsersService usersService)
    {
        _configuration = configuration;
        _usersService = usersService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
        string passwordHash
            = BCrypt.Net.BCrypt.HashPassword(request.Password);

        user.Username = request.Username;
        user.PasswordHash = passwordHash;

        try
        {
            await _usersService.CreateAsync(user);
            return Ok(user);
        }
        catch
        {
            return BadRequest("Invalid Username or Password");
        };
    }


    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(UserDto request)
    {
        var user = await _usersService.GetAsync(request.Username);

        if (user == null)
        {
            return BadRequest("Username or password is incorrect.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Username or password is incorrect.");
        }

        string token = CreateToken(user);

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("JwtSettings:Key").Value!
        ));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

}