using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Responses;
using BlogApi.Application.Validators;
using BlogApi.Web.Common;
using FluentValidation;

namespace BlogApi.Web.Endpoints.Posts;

public class AddCommentEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id:guid}/comments", HandleAsync)
            .WithName("AddComment")
            .WithOpenApi()
            .Produces<Response<CommentDto>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

    private static async Task<IResult> HandleAsync(
        Guid id,
        CreateCommentDto commentDto,
        CreateCommentDtoValidator validator,
        IBlogService blogService)
    {
        var validationResult = await validator.ValidateAsync(commentDto);
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

        var comment = await blogService.AddCommentAsync(id, commentDto);
        return TypedResults.Created(
            $"/api/posts/{id}",
            new Response<CommentDto>(
                comment,
                StatusCodes.Status201Created,
                "Comment added successfully"));
    }
}
