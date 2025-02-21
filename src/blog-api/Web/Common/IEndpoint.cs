namespace BlogApi.Web.Common;

public interface IEndpoint
{
    static abstract void Map(IEndpointRouteBuilder app);
}
