using System.ComponentModel.DataAnnotations;
namespace SkillMentor.dto;
public class StudentDTO
{
    [StringLength(500, ErrorMessage = "Learning goals must not exceed 500 characters")]
    public string? LearningGoals { get; set; }
}
