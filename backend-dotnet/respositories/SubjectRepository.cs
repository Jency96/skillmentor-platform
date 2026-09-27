using Microsoft.EntityFrameworkCore;
using SkillMentor.entities;
namespace SkillMentor.respositories;
public class SubjectRepository(SkillMentorDbContext db)
{
    public Task<Subject?> FindById(long id) => db.Subjects.FirstOrDefaultAsync(x => x.Id == id);
    public Task<List<Subject>> FindAll() => db.Subjects.AsNoTracking().ToListAsync();
    public async Task<Subject> Save(Subject subject) { if (subject.Id == 0) db.Subjects.Add(subject); await db.SaveChangesAsync(); return subject; }
    public async Task DeleteById(long id) { var subject = await FindById(id); if (subject is null) return; db.Subjects.Remove(subject); await db.SaveChangesAsync(); }
}
