using Microsoft.EntityFrameworkCore;
using Session = SkillMentor.entities.Session;
namespace SkillMentor.respositories;
public class SessionRepository(SkillMentorDbContext db)
{
    private IQueryable<Session> WithRelations() => db.Sessions.Include(x => x.Student).Include(x => x.Mentor).Include(x => x.Subject);
    public Task<Session?> FindById(long id) => WithRelations().FirstOrDefaultAsync(x => x.Id == id);
    public Task<List<Session>> FindAll() => WithRelations().AsNoTracking().ToListAsync();
    public Task<List<Session>> FindByStudent_Email(string email) => WithRelations().Where(x => x.Student.Email == email).AsNoTracking().ToListAsync();
    public async Task<Session> Save(Session session) { if (session.Id == 0) db.Sessions.Add(session); await db.SaveChangesAsync(); return session; }
    public async Task DeleteById(long id) { var session = await FindById(id); if (session is null) return; db.Sessions.Remove(session); await db.SaveChangesAsync(); }
}
