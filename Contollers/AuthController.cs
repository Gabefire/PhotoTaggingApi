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
public class AuthController(IConfiguration configuration, UsersService usersService) : ControllerBase
{
    private static User user = new();
    private readonly IConfiguration _configuration = configuration;
    private readonly UsersService _usersService = usersService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
        if (request.Username == null || request.Password == null)
        {
            return BadRequest("Please include a username and password");
        }

        string passwordHash
            = BCrypt.Net.BCrypt.HashPassword(request.Password);

        user.Username = request.Username;
        user.PasswordHash = passwordHash;

        try
        {
            // Check if username is taken
            var userTest = await _usersService.GetAsync(request.Username) ?? throw new Exception("Name already token");

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
            return BadRequest("No user");
        }

        if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Username or password is incorrect.");
        }

        string token = CreateToken(user);

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        ];

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