namespace BlogApi.Application.DTOs;

public record CommentDto(
    Guid Id,
    string Content,
    DateTime CreatedAt);

public record CreateCommentDto(
    string Content);