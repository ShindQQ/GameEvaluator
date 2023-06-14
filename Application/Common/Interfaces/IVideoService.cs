namespace Application.Common.Interfaces;

public interface IVideoService
{
    FileStream StreamVideo();

    bool IsFileExists();
}
