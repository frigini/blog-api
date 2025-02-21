using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Responses;
using BlogApi.Application.Validators;
using BlogApi.Web.Common;
using FluentValidation;

namespace BlogApi.Web.Endpoints;

public class CreatePostEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("CreatePost")
            .WithOpenApi()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<Response<BlogPostDto>>(StatusCodes.Status201Created);

    private static async Task<IResult> HandleAsync(
        CreateBlogPostDto postDto,
        CreateBlogPostDtoValidator validator,
        IBlogService blogService)
    {
        var validationResult = await validator.ValidateAsync(postDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
                .ToList();

            return TypedResults.BadRequest(new Response<ValidationErrorResponse>(
                new ValidationErrorResponse(errors),
                StatusCodes.Status400BadRequest,
                "Validation failed"));
        }

        var post = await blogService.CreatePostAsync(postDto);
        return TypedResults.Created(
            $"/api/posts/{post.Id}",
            new Response<BlogPostDto>(
                post,
                201,
                "Post created successfully"));
    }
}
