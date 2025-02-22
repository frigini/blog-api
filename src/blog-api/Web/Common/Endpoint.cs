using BlogApi.Web.Endpoints.Authentication;
using BlogApi.Web.Endpoints.Posts;

namespace BlogApi.Web.Common;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("/api/v1/auth")
            .WithTags("Authentication")
            .WithOpenApi()
            .MapEndpoint<AuthenticationEndpoint>();

        endpoints.MapGroup("/api/v1/posts")
            .WithTags("Posts")
            .WithOpenApi()
            .RequireAuthorization()
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
