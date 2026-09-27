using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SkillMentor.entities;
using SkillMentor.exceptions;
using SkillMentor.respositories;
namespace SkillMentor.services.impl;
public class MentorServiceImpl(MentorRepository repository, IDistributedCache cache) : MentorService
{
    public async Task<Mentor> CreateNewMentor(Mentor mentor)
    {
        try { var result = await repository.Save(mentor); await cache.RemoveAsync("mentors"); return result; }
        catch (DbUpdateException) { throw new SkillMentorException("Mentor with this email already exists", 409); }
    }
    public async Task<object> GetAllMentors(string? name, int page, int size)
    {
        var query = string.IsNullOrEmpty(name) ? repository.FindAll() : repository.FindByName(name);
        var total = await query.CountAsync();
        var content = await query.Skip(Math.Max(0, page) * size).Take(size).ToListAsync();
        return new { content, pageable = new { pageNumber = page, pageSize = size }, totalElements = total,
            totalPages = (int)Math.Ceiling((double)total / size), size, number = page, numberOfElements = content.Count,
            first = page == 0, last = (page + 1) * size >= total, empty = content.Count == 0 };
    }
    public async Task<Mentor> GetMentorById(long id) => await repository.FindById(id) ?? throw new SkillMentorException("Mentor Not found", 404);
    public async Task<Mentor> UpdateMentorById(long id, Mentor updated)
    {
        var mentor = await GetMentorById(id);
        // ModelMapper's skip-null setting: only supplied values replace existing values.
        mentor.MentorId = updated.MentorId.Length > 0 ? updated.MentorId : mentor.MentorId;
        mentor.FirstName = updated.FirstName.Length > 0 ? updated.FirstName : mentor.FirstName;
        mentor.LastName = updated.LastName.Length > 0 ? updated.LastName : mentor.LastName;
        mentor.Email = updated.Email.Length > 0 ? updated.Email : mentor.Email;
        mentor.PhoneNumber = updated.PhoneNumber ?? mentor.PhoneNumber; mentor.Title = updated.Title ?? mentor.Title;
        mentor.Profession = updated.Profession ?? mentor.Profession; mentor.Company = updated.Company ?? mentor.Company;
        mentor.ExperienceYears = updated.ExperienceYears; mentor.Bio = updated.Bio ?? mentor.Bio;
        mentor.ProfileImageUrl = updated.ProfileImageUrl ?? mentor.ProfileImageUrl;
        mentor.PositiveReviews = updated.PositiveReviews ?? mentor.PositiveReviews;
        mentor.TotalEnrollments = updated.TotalEnrollments ?? mentor.TotalEnrollments;
        mentor.IsCertified = updated.IsCertified ?? mentor.IsCertified; mentor.StartYear = updated.StartYear ?? mentor.StartYear;
        try { var result = await repository.Save(mentor); await cache.RemoveAsync("mentors"); return result; }
        catch (DbUpdateException) { throw new SkillMentorException("Failed to update mentor", 500); }
    }
    public async Task DeleteMentor(long id) { await repository.DeleteById(id); await cache.RemoveAsync("mentors"); }
}
