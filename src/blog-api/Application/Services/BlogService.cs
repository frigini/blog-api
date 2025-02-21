using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Exceptions;
using BlogApi.Domain.Interface;

namespace BlogApi.Application.Services;

public class BlogService(IBlogRepository repository) : IBlogService
{
    public async Task<IEnumerable<BlogPostDto>> GetAllPostsAsync()
    {
        var posts = await repository.GetAllPostsAsync();
        return posts.Select(p => new BlogPostDto(
            p.Id,
            p.Title,
            p.Content,
            p.CreatedAt,
            p.UpdatedAt,
            p.Comments.Count));
    }

    public async Task<BlogPostDetailDto> GetPostByIdAsync(Guid id)
    {
        var post = await repository.GetPostByIdAsync(id) ?? throw new NotFoundException($"Blog post with ID {id} was not found");

        return new BlogPostDetailDto(
            post.Id,
            post.Title,
            post.Content,
            post.CreatedAt,
            post.UpdatedAt,
            post.Comments.Select(c => new CommentDto(c.Id, c.Content, c.CreatedAt)));
    }

    public async Task<BlogPostDto> CreatePostAsync(CreateBlogPostDto postDto)
    {
        if (await repository.TitleAlreadyExistsAsync(postDto.Title))
            throw new DomainException($"A post with the title '{postDto.Title}' already exists");

        var post = new BlogPost
        {
            Id = Guid.NewGuid(),
            Title = postDto.Title,
            Content = postDto.Content,
            CreatedAt = DateTime.UtcNow
        };

        var created = await repository.CreatePostAsync(post);
        await repository.SaveChangesAsync();

        return new BlogPostDto(
            created.Id,
            created.Title,
            created.Content,
            created.CreatedAt,
            created.UpdatedAt,
            0);
    }

    public async Task<CommentDto> AddCommentAsync(Guid postId, CreateCommentDto commentDto)
    {
        _ = await repository.GetPostByIdAsync(postId) ?? throw new NotFoundException($"Blog post with ID {postId} was not found");

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Content = commentDto.Content,
            CreatedAt = DateTime.UtcNow,
            BlogPostId = postId
        };

        var created = await repository.AddCommentAsync(comment);
        await repository.SaveChangesAsync();

        return new CommentDto(
            created.Id,
            created.Content,
            created.CreatedAt);
    }
}