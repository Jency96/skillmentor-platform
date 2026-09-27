using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillMentor.dto;
using SkillMentor.entities;
using SkillMentor.security;
using SkillMentor.services;
namespace SkillMentor.controllers;
[ApiController, Authorize]
[Route("api/v1/students")]
public class StudentController(StudentService studentService) : AbstractController
{
    [HttpGet, Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<List<Student>>> GetAllStudents() => SendOkResponse(await studentService.GetAllStudents());
    [HttpGet("{id:int}"), Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<Student>> GetStudentById(int id) => SendOkResponse(await studentService.GetStudentById(id));
    [HttpPost, Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<Student>> CreateStudent([FromBody] StudentDTO dto)
    {
        var principal = UserPrincipal.FromClaims(User);
        return SendCreatedResponse(await studentService.CreateNewStudent(new Student
        { StudentId = principal.Id ?? "", FirstName = principal.FirstName ?? "", LastName = principal.LastName ?? "", Email = principal.Email ?? "", LearningGoals = dto.LearningGoals }));
    }
    [HttpPut("{id:int}"), Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<Student>> UpdateStudent(int id, [FromBody] StudentDTO dto)
        => SendOkResponse(await studentService.UpdateStudentById(id, new Student { LearningGoals = dto.LearningGoals }));
    [HttpDelete("{id:int}"), Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteStudent(int id) { await studentService.DeleteStudent(id); return NoContent(); }
}
