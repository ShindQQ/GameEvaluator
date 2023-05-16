namespace Application.Common.Exceptions;

public sealed class PasswordException : Exception
{
    public PasswordException()
    {
    }

    public PasswordException(string? message)
        : base(message)
    {
    }

    public PasswordException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public PasswordException(string name, object key)
        : base($"Password \"{name}\" ({key}) is incorrect!")
    {
    }
}
