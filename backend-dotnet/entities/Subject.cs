using System.Text.Json.Serialization;
namespace SkillMentor.entities;
public class Subject
{
    public long Id { get; set; }
    public string SubjectName { get; set; } = "";
    public string Description { get; set; } = "";
    public string? CourseImageUrl { get; set; }
    [JsonIgnore] public Mentor Mentor { get; set; } = null!;
    public long MentorDbId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
