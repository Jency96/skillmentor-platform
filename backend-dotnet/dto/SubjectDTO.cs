using System.ComponentModel.DataAnnotations;
namespace SkillMentor.dto;
public class SubjectDTO
{
    [Required(ErrorMessage = "cannot be null")]
    [MinLength(5, ErrorMessage = "Subject must be at least 5 characters long")]
    public string? SubjectName { get; set; }
    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
    public string? Description { get; set; }
    public string? CourseImageUrl { get; set; }
    [Required] public long? MentorId { get; set; }
}
