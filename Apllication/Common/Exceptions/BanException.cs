namespace Application.Common.Exceptions;

public sealed class BanException : Exception
{
    public BanException()
    {
    }

    public BanException(string? message)
        : base(message)
    {
    }

    public BanException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public BanException(string name, object key)
        : base($"User \"{name}\" ({key}) is banned!")
    {
    }
}
