namespace SkillMentor.dto;
public class ErrorResponse
{
    public string Message { get; set; } = "";
    public string ErrorCode { get; set; } = "";
    public string Timestamp { get; set; } = DateTime.Now.ToString("O");
    public Dictionary<string, string> ValidationErrors { get; set; } = new();
}
