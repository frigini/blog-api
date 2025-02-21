using BlogApi.Application.DTOs;

namespace BlogApi.Application.Interfaces;

public interface IBlogService
{
    Task<IEnumerable<BlogPostDto>> GetAllPostsAsync();
    Task<BlogPostDetailDto> GetPostByIdAsync(Guid id);
    Task<BlogPostDto> CreatePostAsync(CreateBlogPostDto postDto);
    Task<CommentDto> AddCommentAsync(Guid postId, CreateCommentDto commentDto);
}