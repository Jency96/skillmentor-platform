using SkillMentor.entities;
namespace SkillMentor.services;
public interface SubjectService
{
    Task<List<Subject>> GetAllSubjects();
    Task<Subject> AddNewSubject(long mentorId, Subject subject);
    Task<Subject> GetSubjectById(long id);
    Task<Subject> UpdateSubjectById(long id, Subject updatedSubject);
    Task DeleteSubject(long id);
}
