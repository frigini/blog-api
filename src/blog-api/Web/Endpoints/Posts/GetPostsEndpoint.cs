using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Responses;
using BlogApi.Web.Common;
using FluentValidation;

namespace BlogApi.Web.Endpoints.Posts;

public class GetPostsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("GetPosts")
            .WithOpenApi()
            .Produces<PagedResponse<IEnumerable<BlogPostDto>>>(StatusCodes.Status200OK);

    private static async Task<IResult> HandleAsync(
        IBlogService blogService)
    {
        var posts = await blogService.GetAllPostsAsync();
        return TypedResults.Ok(new PagedResponse<IEnumerable<BlogPostDto>>(
            posts,
            200,
            "Posts retrieved successfully"));
    }
}
