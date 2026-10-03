using Peapoll.Models;

namespace Peapoll.Services;

public class PollService
{
    private readonly List<Poll> _polls = new();

    public PollService()
    {
        _polls.Add(new Poll
        {
            Question = "What framework are you using for Peapoll?",
            CreatedBy = "Hertz",
            CreatedAt = DateTime.UtcNow.AddHours(-3),
            ExpiresAt = DateTime.UtcNow.AddDays(2),
            Options = new List<PollOption>
            {
                new() { Text = "Blazor Interactive Server", Votes = 14 },
                new() { Text = "Blazor WebAssembly", Votes = 8 },
                new() { Text = "Next.js", Votes = 5 }
            }
        });

        _polls.Add(new Poll
        {
            Question = "Should anonymous users see live poll results before voting?",
            CreatedBy = "Hertz",
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            ExpiresAt = DateTime.UtcNow.AddHours(12),
            Options = new List<PollOption>
            {
                new() { Text = "Yes, keep it open", Votes = 22 },
                new() { Text = "No, hide until vote is submitted", Votes = 19 }
            }
        });
    }

    public List<Poll> GetAllPolls() => _polls.OrderByDescending(p => p.CreatedAt).ToList();

    public List<Poll> GetPollsByUser(string username) => 
        _polls.Where(p => p.CreatedBy.Equals(username, StringComparison.OrdinalIgnoreCase))
              .OrderByDescending(p => p.CreatedAt)
              .ToList();

    public void CreatePoll(string question, List<string> optionTexts, DateTime? expiresAt, string author)
    {
        var poll = new Poll
        {
            Question = question,
            CreatedBy = author,
            ExpiresAt = expiresAt,
            Options = optionTexts.Where(t => !string.IsNullOrWhiteSpace(t))
                                 .Select(t => new PollOption { Text = t })
                                 .ToList()
        };
        _polls.Add(poll);
    }

    public bool Vote(Guid pollId, Guid optionId)
    {
        var poll = _polls.FirstOrDefault(p => p.Id == pollId);
        if (poll == null || poll.IsExpired) return false;

        var option = poll.Options.FirstOrDefault(o => o.Id == optionId);
        if (option == null) return false;

        option.Votes++;
        return true;
    }
}