namespace Domain.ValueObjects;

public record Ban
{
    public static Ban Create(DateTime bannedTo) => new()
    {
        IsBaned = true,
        BannedAt = DateTime.UtcNow,
        BannedTo = bannedTo
    };

    public bool IsBaned { get; set; }

    public DateTime BannedAt { get; set; }

    public DateTime BannedTo { get; set; }
}
