using SkillMentor.dto;
using SkillMentor.exceptions;
using SkillMentor.respositories;
using SkillMentor.security;
using SkillMentor.utils;
using Session = SkillMentor.entities.Session;
using Student = SkillMentor.entities.Student;
namespace SkillMentor.services.impl;
public class SessionServiceImpl(SessionRepository sessions, StudentRepository students, MentorRepository mentors, SubjectRepository subjects) : SessionService
{
    public async Task<Session> CreateNewSession(SessionDTO dto)
    {
        var student = await students.FindById(dto.StudentId ?? 0) ?? throw new SkillMentorException("Student not found", 404);
        var mentor = await mentors.FindByMentorId(dto.MentorId?.ToString() ?? "") ?? throw new SkillMentorException("Mentor not found", 404);
        var subject = await subjects.FindById(dto.SubjectId ?? 0) ?? throw new SkillMentorException("Subject not found", 404);
        ValidationUtils.ValidateMentorAvailability(mentor, dto.SessionAt!.Value, dto.DurationMinutes);
        ValidationUtils.ValidateStudentAvailability(student, dto.SessionAt.Value, dto.DurationMinutes);
        return await sessions.Save(new Session { Student = student, Mentor = mentor, Subject = subject,
            SessionAt = dto.SessionAt.Value, DurationMinutes = dto.DurationMinutes, SessionStatus = dto.SessionStatus,
            MeetingLink = dto.MeetingLink, SessionNotes = dto.SessionNotes, StudentReview = dto.StudentReview,
            StudentRating = dto.StudentRating, PaymentStatus = dto.PaymentStatus });
    }
    public Task<List<Session>> GetAllSessions() => sessions.FindAll();
    public async Task<Session> GetSessionById(long id) => await sessions.FindById(id) ?? throw new SkillMentorException("Session not found", 404);
    public async Task<Session> UpdateSessionById(long id, SessionDTO dto)
    {
        var session = await GetSessionById(id);
        if (dto.StudentId is int studentId) session.Student = await students.FindById(studentId) ?? throw new SkillMentorException("Student not found", 404);
        if (dto.MentorId is long mentorId) session.Mentor = await mentors.FindByMentorId(mentorId.ToString()) ?? throw new SkillMentorException("Mentor not found", 404);
        if (dto.SubjectId is long subjectId) session.Subject = await subjects.FindById(subjectId) ?? throw new SkillMentorException("Subject not found", 404);
        if (dto.SessionAt is DateTime date) session.SessionAt = date;
        session.DurationMinutes = dto.DurationMinutes ?? session.DurationMinutes;
        session.SessionStatus = dto.SessionStatus ?? session.SessionStatus;
        session.MeetingLink = dto.MeetingLink ?? session.MeetingLink;
        session.SessionNotes = dto.SessionNotes ?? session.SessionNotes;
        session.StudentReview = dto.StudentReview ?? session.StudentReview;
        session.StudentRating = dto.StudentRating ?? session.StudentRating;
        session.PaymentStatus = dto.PaymentStatus ?? session.PaymentStatus;
        return await sessions.Save(session);
    }
    public Task DeleteSession(long id) => sessions.DeleteById(id);
    public async Task<Session> EnrollSession(UserPrincipal principal, SessionDTO dto)
    {
        var student = await students.FindByEmail(principal.Email ?? "") ?? await students.Save(new Student
        { StudentId = principal.Id ?? "", Email = principal.Email ?? "", FirstName = principal.FirstName ?? "", LastName = principal.LastName ?? "" });
        var mentor = await mentors.FindByMentorId(dto.MentorId?.ToString() ?? "") ?? throw new SkillMentorException("Mentor not found", 404);
        var subject = await subjects.FindById(dto.SubjectId ?? 0) ?? throw new SkillMentorException("Subject not found", 404);
        return await sessions.Save(new Session { Student = student, Mentor = mentor, Subject = subject,
            SessionAt = dto.SessionAt!.Value, DurationMinutes = dto.DurationMinutes ?? 60,
            SessionStatus = "scheduled", PaymentStatus = "pending" });
    }
    public Task<List<Session>> GetSessionsByStudentEmail(string email) => sessions.FindByStudent_Email(email);
}
