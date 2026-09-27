namespace SkillMentor.exceptions;
public class SkillMentorException(string message, int status) : Exception(message)
{
    public int Status { get; } = status;
}
