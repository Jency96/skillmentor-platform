namespace SkillMentor.entities;
public class Student
{
    public int Id { get; set; }
    public string StudentId { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? LearningGoals { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<Session> Sessions { get; set; } = new();
}
