using Application.Common.Interfaces;
using NReco.VideoConverter;
using NReco.VideoInfo;

namespace Presentation.API.Services;

public class VideoService : IVideoService
{
    private readonly string _inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"videos/video1.mp4");
    private readonly string _outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"videos/out.mp4");
    private static readonly DateTime _startedAt;

    static VideoService()
    {
        _startedAt = DateTime.Now;
    }

    public FileStream StreamVideo()
    {
        var ffProbe = new FFProbe();
        var videoInfo = ffProbe.GetMediaInfo(_inputFilePath);
        var videoDuration = videoInfo.Duration.TotalSeconds;

        var videosPlayedAmount = (DateTime.Now - _startedAt).TotalSeconds / videoDuration;
        var roundedAmount = Math.Floor(videosPlayedAmount);
        var seekTime = (float)Math.Floor((videosPlayedAmount - roundedAmount) * videoDuration);

        var ffMpeg = new FFMpegConverter();
        ffMpeg.ConvertMedia(_inputFilePath, Format.mp4, _outputFilePath, Format.mp4, new ConvertSettings
        {
            Seek = seekTime,
            AudioCodec = "copy",
            VideoCodec = "copy",
            MaxDuration = (float)videoDuration,
        });

        return File.OpenRead(_outputFilePath);
    }

    public bool IsFileExists()
    {
        return File.Exists(_inputFilePath);
    }
}
