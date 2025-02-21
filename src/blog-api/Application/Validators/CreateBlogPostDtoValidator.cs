using BlogApi.Application.DTOs;
using FluentValidation;

namespace BlogApi.Application.Validators;

public class CreateBlogPostDtoValidator : AbstractValidator<CreateBlogPostDto>
{
    public CreateBlogPostDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Post title cannot be empty")
            .MinimumLength(3).WithMessage("Post title must be at least 3 characters")
            .MaximumLength(200).WithMessage("Post title cannot exceed 200 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Post content cannot be empty")
            .MinimumLength(10).WithMessage("Post content must be at least 10 characters");
    }
}