using BlogApi.Web.Endpoints;

namespace BlogApi.Web.Common;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("v1/api/posts")
            .WithTags("Posts")
            .MapEndpoint<AddCommentEndpoint>()
            .MapEndpoint<CreatePostEndpoint>()
            .MapEndpoint<GetPostEndpoint>()
            .MapEndpoint<GetPostsEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
