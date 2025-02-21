using BlogApi.Domain.Entities;
using BlogApi.Domain.Interface;
using BlogApi.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infra.Repositories;

public class BlogRepository(BlogContext context) : IBlogRepository
{
    public async Task<IEnumerable<BlogPost>> GetAllPostsAsync()
    {
        return await context.BlogPosts
            .Include(x => x.Comments)
            .ToListAsync();
    }

    public async Task<BlogPost?> GetPostByIdAsync(Guid id)
    {
        return await context.BlogPosts
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BlogPost> CreatePostAsync(BlogPost post)
    {
        await context.BlogPosts.AddAsync(post);
        return post;
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        return comment;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public async Task<bool> TitleAlreadyExistsAsync(string title)
    {
        return await context.BlogPosts
            .AnyAsync(p => p.Title.ToLower() == title.ToLower());
    }
}