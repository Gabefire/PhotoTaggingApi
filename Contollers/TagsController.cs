using PhotoTaggingApi.Models;
using PhotoTaggingApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace PhotoTaggingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly TagsService _tagsService;

    public TagsController(TagsService tagsService) =>
        _tagsService = tagsService;

    [HttpGet]
    public async Task<List<Tag>> Get() =>
        await _tagsService.GetAsync();
}