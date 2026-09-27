using System.ComponentModel.DataAnnotations;
namespace SkillMentor.dto;
public class MentorDTO
{
    public string? MentorId { get; set; }
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters")] public string? FirstName { get; set; }
    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters")] public string? LastName { get; set; }
    [EmailAddress(ErrorMessage = "Email must be valid")] public string? Email { get; set; }
    [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters")] public string? PhoneNumber { get; set; }
    [StringLength(100, ErrorMessage = "Title must not exceed 100 characters")] public string? Title { get; set; }
    [StringLength(100, ErrorMessage = "Profession must not exceed 100 characters")] public string? Profession { get; set; }
    [StringLength(100, ErrorMessage = "Company must not exceed 100 characters")] public string? Company { get; set; }
    public int ExperienceYears { get; set; }
    [StringLength(500, ErrorMessage = "Bio must not exceed 500 characters")] public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int? PositiveReviews { get; set; }
    public int? TotalEnrollments { get; set; }
    public bool? IsCertified { get; set; }
    [StringLength(10, ErrorMessage = "Start year must not exceed 10 characters")] public string? StartYear { get; set; }
}
