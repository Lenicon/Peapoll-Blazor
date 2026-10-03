namespace Peapoll.Models;

public class PollOption
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
    public int Votes { get; set; }
}

public class Poll
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Question { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = "Anonymous";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public List<PollOption> Options { get; set; } = new();

    public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
    public int TotalVotes => Options.Sum(o => o.Votes);
}