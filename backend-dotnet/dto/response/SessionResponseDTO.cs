namespace SkillMentor.dto.response;
public class SessionResponseDTO
{
    public int Id { get; set; }
    public string MentorName { get; set; } = "";
    public string? MentorProfileImageUrl { get; set; }
    public string SubjectName { get; set; } = "";
    public DateTime SessionAt { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? MeetingLink { get; set; }
}
