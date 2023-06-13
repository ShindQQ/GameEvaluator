using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NReco.VideoConverter;

namespace Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class VideosController : ControllerBase
{
    private const string inputFilePath = "../videos/video.mp4";
    private const string outputFilePath = "../videos/out.mp4";

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        if (!System.IO.File.Exists(inputFilePath))
        {
            return BadRequest();
        }

        var ffMpeg = new FFMpegConverter();

        ffMpeg.ConvertMedia(inputFilePath, Format.mp4, outputFilePath, Format.mp4, new ConvertSettings
        {
            Seek = (float)TimeSpan.FromSeconds(2).TotalSeconds,
            VideoCodec = "libx264",
            AudioCodec = "copy"
        });

        var fileStream = System.IO.File.OpenRead(outputFilePath);

        Response.Headers.Add("Accept-Ranges", "bytes");
        return new FileStreamResult(fileStream, new MediaTypeHeaderValue("video/mp4"))
        {
            EnableRangeProcessing = true
        };
    }
}
