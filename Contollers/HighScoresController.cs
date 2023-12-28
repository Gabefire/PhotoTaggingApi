using PhotoTaggingApi.Models;
using PhotoTaggingApi.Services;
using Microsoft.AspNetCore.Mvc;
using SessionVariable;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration.UserSecrets;


namespace PhotoTaggingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HighScoreController(HighScoreService highScoreService) : ControllerBase
{
    public static HighScore highScore = new HighScore();
    private readonly HighScoreService _highScoreService = highScoreService;

    [HttpGet]
    public async Task<List<HighScore>> Get() =>
        await _highScoreService.GetAsync();

    [HttpPost]
    public async Task<IActionResult> AddHighScore()
    {
        try
        {
            //JWT for user ID

            string token = HttpContext.Request.Headers.Authorization.ToString();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token.Split(" ")[1]) as JwtSecurityToken ?? throw new Exception("No token");

            var userId = jwtToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;


            //Session section for date time
            var sessionTime = HttpContext.Session.GetString(SessionKeyEnum.SessionKeySessionDate.ToString()) ?? throw new ArgumentException("Session time cannot be null");

            DateTime parsedDateTime = DateTime.Parse(sessionTime!);

            DateTime endTime = DateTime.UtcNow;

            TimeSpan ts = endTime - parsedDateTime;

            highScore.Time = ts.TotalMilliseconds;
            highScore.UserId = userId;

            await _highScoreService.CreateAsync(highScore);

            return Ok(highScore);
        }
        catch
        {
            return BadRequest("Invalid token");
        }
    }
}