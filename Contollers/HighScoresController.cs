using PhotoTaggingApi.Models;
using PhotoTaggingApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace PhotoTaggingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HighScoreController : ControllerBase
{
    public static HighScore highScore = new HighScore();
    private readonly HighScoreService _highScoreService;

    public HighScoreController(HighScoreService highScoreService) =>
        _highScoreService = highScoreService;

    [HttpGet]
    public async Task<List<HighScore>> Get() =>
        await _highScoreService.GetAsync();

    [HttpPost]
    public async Task<IActionResult> AddHighScore()
    {


        try
        {
            await _highScoreService.CreateAsync(highScore);
        }
        catch
        {
            return BadRequest("Invalid high score");
        }
        return Ok();
    }
}