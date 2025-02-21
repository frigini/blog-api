using System.Text.Json.Serialization;

namespace BlogApi.Application.Responses;

public class Response<TData>
{
    private readonly int _code;
    private const int DefaultStatusCode = 200;

    [JsonConstructor]
    public Response() => _code = DefaultStatusCode;

    public Response(
        TData? data,
        int statusCode = DefaultStatusCode,
        string? message = null)
    {
        Data = data;
        _code = statusCode;
        Message = message;
    }

    [JsonIgnore]
    public bool IsSuccess => _code is >= 200 and <= 299;
    public string? Message { get; set; }
    public TData? Data { get; set; }
}