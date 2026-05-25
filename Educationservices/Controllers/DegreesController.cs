using Educationservices.DTOs;
using Educationservices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Educationservices.Controllers
{
    // No [Authorize] yet — there is no auth scheme registered (see comment in AssignmentsController).
    [ApiController]
    [Route("api")]
    public class DegreesController : ControllerBase
    {
        private readonly DegreeService _service;

        public DegreesController(DegreeService service)
        {
            _service = service;
        }

        [HttpGet("courses/{courseId}/degrees")]
        public async Task<IActionResult> GetDegreesForCourse(Guid courseId)
        {
            var list = await _service.GetDegreesForCourseAsync(courseId);
            return Ok(list);
        }

        [HttpGet("degrees/me")]
        public async Task<IActionResult> GetMyDegrees()
        {
            // TODO: pull studentId from auth claims once auth is wired up.
            var studentId = HttpContext.User?.Identity?.Name ?? Request.Query["studentId"].ToString();
            if (string.IsNullOrEmpty(studentId)) return BadRequest(new { error = "studentId required" });
            var list = await _service.GetDegreesForStudentAsync(studentId);
            return Ok(list);
        }

        [HttpPost("courses/{courseId}/degrees")]
        public async Task<IActionResult> AssignDegree(Guid courseId, [FromBody] AssignDegreeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.StudentId))
                return BadRequest(new { error = "studentId required" });
            if (!DegreeService.AllowedGrades.Contains(dto.Grade))
                return BadRequest(new { error = "grade must be one of A, B, C, D, E, F" });

            var degree = await _service.AssignDegreeAsync(courseId, dto);
            return Ok(degree);
        }
    }
}
