using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interface;

public interface IBlogRepository
{
    Task<IEnumerable<BlogPost>> GetAllPostsAsync();
    Task<BlogPost?> GetPostByIdAsync(Guid id);
    Task<BlogPost> CreatePostAsync(BlogPost post);
    Task<Comment> AddCommentAsync(Comment comment);
    Task<int> SaveChangesAsync();
    Task<bool> TitleAlreadyExistsAsync(string title);
}