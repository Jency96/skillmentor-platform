using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillMentor.dto;
using SkillMentor.entities;
using SkillMentor.services;
namespace SkillMentor.controllers;
[ApiController, Authorize(Roles = "ADMIN")]
[Route("api/v1/subjects")]
public class SubjectController(SubjectService subjectService) : AbstractController
{
    [HttpGet]
    public async Task<ActionResult<List<Subject>>> GetAllSubjects() => SendOkResponse(await subjectService.GetAllSubjects());
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Subject>> GetSubjectById(long id) => SendOkResponse(await subjectService.GetSubjectById(id));
    [HttpPost]
    public async Task<ActionResult<Subject>> CreateSubject([FromBody] SubjectDTO dto)
        => SendOkResponse(await subjectService.AddNewSubject(dto.MentorId!.Value, Map(dto)));
    [HttpPut("{id:long}")]
    public async Task<ActionResult<Subject>> UpdateSubject(long id, [FromBody] SubjectDTO dto)
        => SendOkResponse(await subjectService.UpdateSubjectById(id, Map(dto)));
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteSubject(long id) { await subjectService.DeleteSubject(id); return Ok(); }
    private static Subject Map(SubjectDTO d) => new() { SubjectName = d.SubjectName ?? "", Description = d.Description ?? "", CourseImageUrl = d.CourseImageUrl };
}
