using SkillMentor.dto;
using SkillMentor.security;
using Session = SkillMentor.entities.Session;
namespace SkillMentor.services;
public interface SessionService
{
    Task<Session> CreateNewSession(SessionDTO dto);
    Task<List<Session>> GetAllSessions();
    Task<Session> GetSessionById(long id);
    Task<Session> UpdateSessionById(long id, SessionDTO dto);
    Task DeleteSession(long id);
    Task<Session> EnrollSession(UserPrincipal principal, SessionDTO dto);
    Task<List<Session>> GetSessionsByStudentEmail(string email);
}
