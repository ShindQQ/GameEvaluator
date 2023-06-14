using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class VideosController : ControllerBase
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        if (!_videoService.IsFileExists())
            return BadRequest();

        Response.Headers.Add("Accept-Ranges", "bytes");
        return new FileStreamResult(_videoService.StreamVideo(), new MediaTypeHeaderValue("video/mp4"))
        {
            EnableRangeProcessing = true
        };
    }
}
