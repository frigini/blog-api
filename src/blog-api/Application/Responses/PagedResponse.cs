using System.Text.Json.Serialization;

namespace BlogApi.Application.Responses;

public class PagedResponse<TData> : Response<TData>
{
    private const int DefaultStatusCode = 200;
    private const int DefaultPageSize = 15;

    [JsonConstructor]
    public PagedResponse(
        TData? data,
        int totalCount,
        int currentPage = 1,
        int pageSize = DefaultPageSize)
        : base(data)
    {
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

    public PagedResponse(
        TData? data,
        int statusCode = DefaultStatusCode,
        string? message = null)
        : base(data, statusCode, message)
    {
    }

    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int PageSize { get; set; } = DefaultPageSize;
    public int TotalCount { get; set; }
}