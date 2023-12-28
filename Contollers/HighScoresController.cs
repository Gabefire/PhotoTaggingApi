using PhotoTaggingApi.Models;
using PhotoTaggingApi.Services;
using Microsoft.AspNetCore.Mvc;
using SessionVariable;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;


namespace PhotoTaggingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HighScoreController(HighScoreService highScoreService, UsersService usersService) : ControllerBase
{
    public static HighScore highScore = new HighScore();
    private readonly HighScoreService _highScoreService = highScoreService;

    private readonly UsersService _usersService = usersService;

    [HttpGet]
    public async Task<List<HighScoreDto>> Get()
    {

        List<HighScoreDto> formattedHighScores = [];
        var highScoreList = await _highScoreService.GetAsync();
        foreach (HighScore highScore in highScoreList)
        {
            HighScoreDto highScoreDto = new();
            string userId = new(highScore.UserId);
            var user = await _usersService.GetAsyncId(userId);
            highScoreDto.UserName = user.Username as string;
            highScoreDto.Time = highScore.Time;

            formattedHighScores.Add(highScoreDto);
        };
        return formattedHighScores;
    }


    [HttpPost, Authorize]
    public async Task<IActionResult> AddHighScore()
    {
        try
        {
            //JWT for user ID

            string token = HttpContext.Request.Headers.Authorization.ToString();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token.Split(" ")[1]) as JwtSecurityToken ?? throw new Exception("No token");

            string userId = jwtToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value ?? throw new Exception("Not a valid ID");

            //Verify user exists

            await _usersService.GetAsyncId(userId);


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