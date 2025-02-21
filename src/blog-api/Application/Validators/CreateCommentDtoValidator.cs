using BlogApi.Application.DTOs;
using FluentValidation;

namespace BlogApi.Application.Validators;

public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment content cannot be empty")
            .MaximumLength(1000).WithMessage("Comment content cannot exceed 1000 characters");
    }
}
