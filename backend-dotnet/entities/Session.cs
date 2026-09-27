using System.Text.Json.Serialization;
namespace SkillMentor.entities;
public class Session
{
    public int Id { get; set; }
    [JsonIgnore] public Student Student { get; set; } = null!;
    public int StudentDbId { get; set; }
    [JsonIgnore] public Mentor Mentor { get; set; } = null!;
    public long MentorDbId { get; set; }
    [JsonIgnore] public Subject Subject { get; set; } = null!;
    public long SubjectDbId { get; set; }
    public DateTime SessionAt { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionStatus { get; set; }
    public string? MeetingLink { get; set; }
    public string? SessionNotes { get; set; }
    public string? StudentReview { get; set; }
    public int? StudentRating { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
