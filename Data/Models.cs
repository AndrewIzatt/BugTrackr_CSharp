using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public enum TicketStatus { New, InProgress, Resolved, Closed, Reopened }
public enum TicketPriority { Low, Medium, High, Critical }
public enum TicketType { Bug, Feature, Task, Chore }

public class Ticket
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = "";

    [StringLength(4000)]
    public string? Description { get; set; }

    [Required] public TicketStatus Status { get; set; } = TicketStatus.New;
    [Required] public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    [Required] public TicketType Type { get; set; } = TicketType.Bug;

    [StringLength(80)] public string? Reporter { get; set; }
    [StringLength(80)] public string? Assignee { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedUtc { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Ticket>()
            .HasIndex(t => new { t.IsDeleted, t.Status, t.Priority });

        // optional: keep Title unique per “active” ticket
        // b.Entity<Ticket>().HasIndex(t => new { t.Title, t.IsDeleted }).IsUnique();
    }
}
