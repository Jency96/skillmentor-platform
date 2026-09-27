using Microsoft.AspNetCore.Mvc;
namespace SkillMentor.controllers;
public abstract class AbstractController : ControllerBase
{
    protected ActionResult<T> SendOkResponse<T>(T response) => Ok(response);
    protected ActionResult<T> SendCreatedResponse<T>(T response) => StatusCode(201, response);
    protected ActionResult SendNotFoundResponse() => NotFound();
    protected ActionResult SendNoContentResponse() => NoContent();
}
