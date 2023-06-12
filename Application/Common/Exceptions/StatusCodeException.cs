using System.Net;

namespace Application.Common.Exceptions;

public sealed class StatusCodeException : Exception
{
    public HttpStatusCode StatusCode { get; set; }

    public StatusCodeException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }

    public StatusCodeException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}