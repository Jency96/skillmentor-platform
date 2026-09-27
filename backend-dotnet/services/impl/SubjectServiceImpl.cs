using Microsoft.EntityFrameworkCore;
using SkillMentor.entities;
using SkillMentor.exceptions;
using SkillMentor.respositories;
namespace SkillMentor.services.impl;
public class SubjectServiceImpl(SubjectRepository repository, MentorRepository mentors) : SubjectService
{
    public Task<List<Subject>> GetAllSubjects() => repository.FindAll();
    public async Task<Subject> AddNewSubject(long mentorId, Subject subject)
    {
        var mentor = await mentors.FindByMentorId(mentorId.ToString()) ?? throw new SkillMentorException("Mentor not found", 404);
        subject.Mentor = mentor;
        try { return await repository.Save(subject); }
        catch (DbUpdateException) { throw new SkillMentorException("Subject already exists or database constraint violation", 409); }
    }
    public async Task<Subject> GetSubjectById(long id) => await repository.FindById(id) ?? throw new SkillMentorException("Subject not found", 404);
    public async Task<Subject> UpdateSubjectById(long id, Subject updated)
    {
        var subject = await GetSubjectById(id);
        subject.SubjectName = updated.SubjectName.Length > 0 ? updated.SubjectName : subject.SubjectName;
        subject.Description = updated.Description.Length > 0 ? updated.Description : subject.Description;
        subject.CourseImageUrl = updated.CourseImageUrl ?? subject.CourseImageUrl;
        try { return await repository.Save(subject); }
        catch (DbUpdateException) { throw new SkillMentorException("Database constraint violation", 409); }
    }
    public Task DeleteSubject(long id) => repository.DeleteById(id);
}
