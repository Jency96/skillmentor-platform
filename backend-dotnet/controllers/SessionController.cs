using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillMentor.dto;
using SkillMentor.dto.response;
using SkillMentor.security;
using SkillMentor.services;
using Session = SkillMentor.entities.Session;
namespace SkillMentor.controllers;
[ApiController, Authorize]
[Route("api/v1/sessions")]
public class SessionController(SessionService sessionService) : AbstractController
{
    [HttpGet, Authorize(Roles = "ADMIN")]
    public Task<List<Session>> GetAllSessions() => sessionService.GetAllSessions();
    [HttpGet("{id:long}"), Authorize(Roles = "ADMIN")]
    public Task<Session> GetSessionById(long id) => sessionService.GetSessionById(id);
    [HttpPost, Authorize(Roles = "ADMIN")]
    public Task<Session> CreateSession([FromBody] SessionDTO dto) => sessionService.CreateNewSession(dto);
    [HttpPut("{id:long}"), Authorize(Roles = "ADMIN")]
    public Task<Session> UpdateSession(long id, [FromBody] SessionDTO dto) => sessionService.UpdateSessionById(id, dto);
    [HttpDelete("{id:long}"), Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteSession(long id) { await sessionService.DeleteSession(id); return Ok(); }
    [HttpPost("enroll"), Authorize(Roles = "STUDENT")]
    public async Task<ActionResult<SessionResponseDTO>> Enroll([FromBody] SessionDTO dto)
        => SendCreatedResponse(ToSessionResponseDTO(await sessionService.EnrollSession(UserPrincipal.FromClaims(User), dto)));
    [HttpGet("my-sessions"), Authorize(Roles = "STUDENT")]
    public async Task<ActionResult<List<SessionResponseDTO>>> GetMySessions()
    {
        var list = await sessionService.GetSessionsByStudentEmail(UserPrincipal.FromClaims(User).Email ?? "");
        return SendOkResponse(list.Select(ToSessionResponseDTO).ToList());
    }
    private static SessionResponseDTO ToSessionResponseDTO(Session session) => new()
    {
        Id = session.Id, MentorName = session.Mentor.FirstName + " " + session.Mentor.LastName,
        MentorProfileImageUrl = session.Mentor.ProfileImageUrl, SubjectName = session.Subject.SubjectName,
        SessionAt = session.SessionAt, DurationMinutes = session.DurationMinutes,
        SessionStatus = session.SessionStatus, PaymentStatus = session.PaymentStatus, MeetingLink = session.MeetingLink
    };
}
