using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;
using SkillMentor.entities;
using SkillMentor.exceptions;
using SkillMentor.respositories;
namespace SkillMentor.services.impl;
public class MentorServiceImpl(MentorRepository repository, IDistributedCache cache, IConfiguration configuration) : MentorService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { ReferenceHandler = ReferenceHandler.IgnoreCycles };
    private bool CacheEnabled => bool.TryParse(configuration["CACHE_ENABLED"], out var enabled) && enabled;
    private async Task<string> CacheKey(string suffix) => $"mentors:{await cache.GetStringAsync("mentors:version") ?? "0"}:{suffix}";
    private Task Invalidate() => CacheEnabled ? cache.SetStringAsync("mentors:version", Guid.NewGuid().ToString("N")) : Task.CompletedTask;

    public async Task<Mentor> CreateNewMentor(Mentor mentor)
    {
        try { var result = await repository.Save(mentor); await Invalidate(); return result; }
        catch (DbUpdateException) { throw new SkillMentorException("Mentor with this email already exists", 409); }
    }
    public async Task<object> GetAllMentors(string? name, int page, int size)
    {
        var cacheKey = CacheEnabled ? await CacheKey($"name:{name ?? ""}:page:{page}:size:{size}") : null;
        if (cacheKey is not null && await cache.GetStringAsync(cacheKey) is string cached)
            return JsonSerializer.Deserialize<JsonElement>(cached);
        var query = string.IsNullOrEmpty(name) ? repository.FindAll() : repository.FindByName(name);
        var total = await query.CountAsync();
        var content = await query.Skip(Math.Max(0, page) * size).Take(size).ToListAsync();
        var result = new { content, pageable = new { pageNumber = page, pageSize = size }, totalElements = total,
            totalPages = (int)Math.Ceiling((double)total / size), size, number = page, numberOfElements = content.Count,
            first = page == 0, last = (page + 1) * size >= total, empty = content.Count == 0 };
        if (cacheKey is not null) await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result, JsonOptions),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
        return result;
    }
    public async Task<Mentor> GetMentorById(long id)
    {
        var cacheKey = CacheEnabled ? await CacheKey($"id:{id}") : null;
        if (cacheKey is not null && await cache.GetStringAsync(cacheKey) is string cached)
            return JsonSerializer.Deserialize<Mentor>(cached, JsonOptions)!;
        var mentor = await repository.FindById(id) ?? throw new SkillMentorException("Mentor Not found", 404);
        if (cacheKey is not null) await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(mentor, JsonOptions),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
        return mentor;
    }
    public async Task<Mentor> UpdateMentorById(long id, Mentor updated)
    {
        var mentor = await repository.FindById(id) ?? throw new SkillMentorException("Mentor Not found", 404);
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
        try { var result = await repository.Save(mentor); await Invalidate(); return result; }
        catch (DbUpdateException) { throw new SkillMentorException("Failed to update mentor", 500); }
    }
    public async Task DeleteMentor(long id) { await repository.DeleteById(id); await Invalidate(); }
}
