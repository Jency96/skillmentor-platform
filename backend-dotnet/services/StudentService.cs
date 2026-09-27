using SkillMentor.entities;
namespace SkillMentor.services;
public interface StudentService
{
    Task<Student> CreateNewStudent(Student student);
    Task<List<Student>> GetAllStudents();
    Task<Student> GetStudentById(int id);
    Task<Student> UpdateStudentById(int id, Student updatedStudent);
    Task DeleteStudent(int id);
}
