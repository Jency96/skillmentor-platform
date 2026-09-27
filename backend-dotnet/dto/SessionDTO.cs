using System.ComponentModel.DataAnnotations;
namespace SkillMentor.dto;
public class SessionDTO
{
    // The Spring enrollment route validates this as required, although its frontend omits it.
    public int? StudentId { get; set; }
    [Required(ErrorMessage = "Mentor ID cannot be null")] public long? MentorId { get; set; }
    [Required(ErrorMessage = "Subject ID cannot be null")] public long? SubjectId { get; set; }
    [Required(ErrorMessage = "Session date/time cannot be null")] public DateTime? SessionAt { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute")] public int? DurationMinutes { get; set; }
    public string? SessionStatus { get; set; }
    public string? MeetingLink { get; set; }
    public string? SessionNotes { get; set; }
    public string? StudentReview { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Rating must be at least 1")] public int? StudentRating { get; set; }
    public string? PaymentStatus { get; set; }
}
