using SkillMentor.entities;
using SkillMentor.exceptions;
using SkillMentor.utils;
using Xunit;
using Session = SkillMentor.entities.Session;
namespace SkillMentor.Tests;
public class ValidationUtilsTest
{
    private static DateTime At(int hour, int minute) => new(2023, 1, 1, hour, minute, 0, DateTimeKind.Utc);
    [Fact] public void IsTimeOverlap_ReturnsTrue_WhenTimesOverlap() =>
        Assert.True(ValidationUtils.IsTimeOverlap(At(10, 0), At(11, 0), At(10, 30), At(11, 30)));
    [Fact] public void IsTimeOverlap_ReturnsFalse_WhenTimesDoNotOverlap() =>
        Assert.False(ValidationUtils.IsTimeOverlap(At(10, 0), At(11, 0), At(11, 0), At(12, 0)));
    [Fact] public void AddMinutesToDate_AddsCorrectMinutes() =>
        Assert.Equal(At(10, 30), ValidationUtils.AddMinutesToDate(At(10, 0), 30));
    [Fact] public void ValidateMentorAvailability_ThrowsException_WhenOverlapExists()
    {
        var mentor = new Mentor { Sessions = [new Session { SessionAt = At(10, 0), DurationMinutes = 60 }] };
        Assert.Throws<SkillMentorException>(() => ValidationUtils.ValidateMentorAvailability(mentor, At(10, 30), 60));
    }
    [Fact] public void ValidateMentorAvailability_DoesNotThrow_WhenNoOverlap()
    {
        var mentor = new Mentor { Sessions = [new Session { SessionAt = At(10, 0), DurationMinutes = 60 }] };
        ValidationUtils.ValidateMentorAvailability(mentor, At(12, 0), 60);
    }
    [Fact] public void ValidateStudentAvailability_ThrowsException_WhenOverlapExists()
    {
        var student = new Student { Sessions = [new Session { SessionAt = At(14, 0), DurationMinutes = 60 }] };
        Assert.Throws<SkillMentorException>(() => ValidationUtils.ValidateStudentAvailability(student, At(14, 30), 60));
    }
}
