using System.ComponentModel.DataAnnotations;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Responses;
using BlogApi.Web.Common;
using FluentValidation;

namespace BlogApi.Web.Endpoints;

public class GetPostEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id:guid}", HandleAsync)
            .WithName("GetPost")
            .WithOpenApi()
            .Produces<Response<BlogPostDetailDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

    private static async Task<IResult> HandleAsync(
        Guid id,
        IBlogService blogService)
    {
        var post = await blogService.GetPostByIdAsync(id);
        return TypedResults.Ok(new Response<BlogPostDetailDto>(
            post,
            200,
            "Post retrieved successfully"));
    }
}
