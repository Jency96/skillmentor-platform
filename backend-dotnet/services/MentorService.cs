using SkillMentor.entities;
namespace SkillMentor.services;
public interface MentorService
{
    Task<Mentor> CreateNewMentor(Mentor mentor);
    Task<object> GetAllMentors(string? name, int page, int size);
    Task<Mentor> GetMentorById(long id);
    Task<Mentor> UpdateMentorById(long id, Mentor updatedMentor);
    Task DeleteMentor(long id);
}
