using System.Text.Json;

namespace Application.Common.Exceptions;

public sealed class StatusCodeErrorDetails
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public StatusCodeErrorDetails(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

}
