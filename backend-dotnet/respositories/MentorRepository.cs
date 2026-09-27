using Microsoft.EntityFrameworkCore;
using SkillMentor.entities;
namespace SkillMentor.respositories;
public class MentorRepository(SkillMentorDbContext db)
{
    public async Task<Mentor?> FindById(long id) => await db.Mentors.Include(x => x.Subjects).Include(x => x.Sessions).FirstOrDefaultAsync(x => x.Id == id);
    public Task<Mentor?> FindByEmail(string email) => db.Mentors.FirstOrDefaultAsync(x => x.Email == email);
    public Task<Mentor?> FindByMentorId(string mentorId) => db.Mentors.Include(x => x.Sessions).FirstOrDefaultAsync(x => x.MentorId == mentorId);
    public IQueryable<Mentor> FindAll() => db.Mentors.Include(x => x.Subjects).AsNoTracking();
    public IQueryable<Mentor> FindByName(string name) => FindAll().Where(x => EF.Functions.ILike(x.FirstName, $"%{name}%") || EF.Functions.ILike(x.LastName, $"%{name}%"));
    public async Task<Mentor> Save(Mentor mentor) { if (mentor.Id == 0) db.Mentors.Add(mentor); await db.SaveChangesAsync(); return mentor; }
    public async Task DeleteById(long id) { var mentor = await FindById(id); if (mentor is null) return; db.Mentors.Remove(mentor); await db.SaveChangesAsync(); }
}
