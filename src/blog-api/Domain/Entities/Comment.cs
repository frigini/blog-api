namespace BlogApi.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; } = null!;
}