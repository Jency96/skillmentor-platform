using Microsoft.EntityFrameworkCore;
using SkillMentor.entities;
namespace SkillMentor.respositories;
public class StudentRepository(SkillMentorDbContext db)
{
    public Task<Student?> FindById(int id) => db.Students.Include(x => x.Sessions).FirstOrDefaultAsync(x => x.Id == id);
    public Task<Student?> FindByEmail(string email) => db.Students.Include(x => x.Sessions).FirstOrDefaultAsync(x => x.Email == email);
    public Task<List<Student>> FindAll() => db.Students.Include(x => x.Sessions).AsNoTracking().ToListAsync();
    public async Task<Student> Save(Student student) { if (student.Id == 0) db.Students.Add(student); await db.SaveChangesAsync(); return student; }
    public async Task DeleteById(int id) { var student = await FindById(id); if (student is null) return; db.Students.Remove(student); await db.SaveChangesAsync(); }
}
