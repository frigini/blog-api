namespace BlogApi.Application.DTOs;

public record BlogPostDto(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int CommentsCount);

public record BlogPostDetailDto(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<CommentDto> Comments);

public record CreateBlogPostDto(
    string Title,
    string Content);
