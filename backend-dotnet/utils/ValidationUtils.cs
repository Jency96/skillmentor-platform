using SkillMentor.entities;
using SkillMentor.exceptions;
namespace SkillMentor.utils;
public static class ValidationUtils
{
    public static void ValidateMentorAvailability(Mentor mentor, DateTime sessionAt, int? durationMinutes)
    {
        var end = AddMinutesToDate(sessionAt, durationMinutes is > 0 ? durationMinutes.Value : 60);
        foreach (var existing in mentor.Sessions)
            if (IsTimeOverlap(sessionAt, end, existing.SessionAt, AddMinutesToDate(existing.SessionAt, existing.DurationMinutes ?? 0)))
                throw new SkillMentorException("Mentor is not available at the requested time", 409);
    }
    public static void ValidateStudentAvailability(Student student, DateTime sessionAt, int? durationMinutes)
    {
        var end = AddMinutesToDate(sessionAt, durationMinutes is > 0 ? durationMinutes.Value : 60);
        foreach (var existing in student.Sessions)
            if (IsTimeOverlap(sessionAt, end, existing.SessionAt, AddMinutesToDate(existing.SessionAt, existing.DurationMinutes ?? 0)))
                throw new SkillMentorException("Student is not available at the requested time", 409);
    }
    public static bool IsTimeOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2) => start1 < end2 && start2 < end1;
    public static DateTime AddMinutesToDate(DateTime date, int minutes) => date.AddMinutes(minutes);
}
