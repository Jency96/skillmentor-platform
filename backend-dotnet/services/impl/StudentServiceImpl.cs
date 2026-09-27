using Microsoft.EntityFrameworkCore;
using SkillMentor.entities;
using SkillMentor.exceptions;
using SkillMentor.respositories;
namespace SkillMentor.services.impl;
public class StudentServiceImpl(StudentRepository repository) : StudentService
{
    public async Task<Student> CreateNewStudent(Student student)
    {
        try { return await repository.Save(student); }
        catch (DbUpdateException) { throw new SkillMentorException("Student with this email already exists", 409); }
    }
    public Task<List<Student>> GetAllStudents() => repository.FindAll();
    public async Task<Student> GetStudentById(int id) => await repository.FindById(id) ?? throw new SkillMentorException("Student not found", 404);
    public async Task<Student> UpdateStudentById(int id, Student updated)
    {
        var student = await GetStudentById(id);
        student.LearningGoals = updated.LearningGoals ?? student.LearningGoals;
        return await repository.Save(student);
    }
    public Task DeleteStudent(int id) => repository.DeleteById(id);
}
