using System.Text.Json.Serialization;
namespace SkillMentor.entities;
public class Mentor
{
    public long Id { get; set; }
    public string MentorId { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public string? Title { get; set; }
    public string? Profession { get; set; }
    public string? Company { get; set; }
    public int ExperienceYears { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int? PositiveReviews { get; set; }
    public int? TotalEnrollments { get; set; }
    public bool? IsCertified { get; set; }
    public string? StartYear { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<Subject> Subjects { get; set; } = new();
    [JsonIgnore] public List<Session> Sessions { get; set; } = new();
}
