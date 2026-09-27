using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillMentor.dto;
using SkillMentor.entities;
using SkillMentor.services;
namespace SkillMentor.controllers;
[ApiController]
[Route("api/v1/mentors")]
public class MentorController(MentorService mentorService) : AbstractController
{
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<object>> GetAllMentors([FromQuery] string? name, [FromQuery] int page = 0, [FromQuery] int size = 20)
        => SendOkResponse(await mentorService.GetAllMentors(name, page, Math.Clamp(size, 1, 1000)));
    [HttpGet("{id:long}"), AllowAnonymous]
    public async Task<ActionResult<Mentor>> GetMentorById(long id) => SendOkResponse(await mentorService.GetMentorById(id));
    [HttpPost, Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<Mentor>> CreateMentor([FromBody] MentorDTO dto)
        => SendCreatedResponse(await mentorService.CreateNewMentor(Map(dto)));
    [HttpPut("{id:long}"), Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<Mentor>> UpdateMentor(long id, [FromBody] MentorDTO dto)
        => SendOkResponse(await mentorService.UpdateMentorById(id, Map(dto)));
    [HttpDelete("{id:long}"), Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteMentor(long id) { await mentorService.DeleteMentor(id); return NoContent(); }
    private static Mentor Map(MentorDTO d) => new()
    {
        MentorId = d.MentorId ?? "", FirstName = d.FirstName ?? "", LastName = d.LastName ?? "", Email = d.Email ?? "",
        PhoneNumber = d.PhoneNumber, Title = d.Title, Profession = d.Profession, Company = d.Company,
        ExperienceYears = d.ExperienceYears, Bio = d.Bio, ProfileImageUrl = d.ProfileImageUrl,
        PositiveReviews = d.PositiveReviews, TotalEnrollments = d.TotalEnrollments, IsCertified = d.IsCertified, StartYear = d.StartYear
    };
}
