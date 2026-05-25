using Educationservices.DTOs;
using Educationservices.Models;
using Educationservices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Educationservices.Controllers
{
    // NOTE: [Authorize] attributes have been removed for the first iteration because
    // no authentication scheme is registered in Program.cs. Re-add them when auth is wired up
    // (JWT bearer or cookie scheme + AddAuthentication / AddAuthorization in Program.cs).
    [ApiController]
    [Route("api")]
    public class AssignmentsController : ControllerBase
    {
        private readonly AssignmentService _service;

        public AssignmentsController(AssignmentService service)
        {
            _service = service;
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _service.GetCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("courses/{courseId}/assignments")]
        public async Task<IActionResult> GetAssignments(Guid courseId)
        {
            var list = await _service.GetAssignmentsForCourseAsync(courseId);
            return Ok(list);
        }

        [HttpGet("courses/{courseId}/assignments/{assignmentId}")]
        public async Task<IActionResult> GetAssignment(Guid courseId, Guid assignmentId)
        {
            var a = await _service.GetAssignmentAsync(courseId, assignmentId);
            if (a == null) return NotFound();
            return Ok(a);
        }

        [HttpPost("courses/{courseId}/assignments")]
        public async Task<IActionResult> CreateAssignment(Guid courseId, [FromBody] CreateAssignmentDto dto)
        {
            if (dto.OpeningDate >= dto.Deadline) return BadRequest(new { error = "openingDate must be before deadline" });
            var created = await _service.CreateAssignmentAsync(courseId, dto);
            return CreatedAtAction(nameof(GetAssignment), new { courseId = courseId, assignmentId = created.Id }, created);
        }

        [HttpPost("assignments/{assignmentId}/submissions")]
        public async Task<IActionResult> SubmitAssignment(Guid assignmentId, [FromBody] SubmitAssignmentDto dto)
        {
            if (dto.Links == null || dto.Links.Count == 0) return BadRequest(new { error = "links required" });
            var sub = await _service.SubmitAssignmentAsync(assignmentId, dto);
            return CreatedAtAction(nameof(GetSubmission), new { assignmentId = assignmentId, submissionId = sub.Id }, sub);
        }

        [HttpGet("assignments/{assignmentId}/submissions")]
        public async Task<IActionResult> GetSubmissions(Guid assignmentId)
        {
            var list = await _service.GetSubmissionsAsync(assignmentId);
            return Ok(list);
        }

        [HttpGet("assignments/{assignmentId}/submissions/me")]
        public async Task<IActionResult> GetMySubmission(Guid assignmentId)
        {
            // TODO: pull studentId from auth claims once auth is wired up.
            var studentId = HttpContext.User?.Identity?.Name ?? Request.Query["studentId"].ToString();
            if (string.IsNullOrEmpty(studentId)) return BadRequest(new { error = "studentId required" });
            var s = await _service.GetStudentSubmissionAsync(assignmentId, studentId);
            if (s == null) return NotFound();
            return Ok(s);
        }

        [HttpGet("assignments/{assignmentId}/submissions/{submissionId}")]
        public async Task<IActionResult> GetSubmission(Guid assignmentId, Guid submissionId)
        {
            var list = await _service.GetSubmissionsAsync(assignmentId);
            var s = list.FirstOrDefault(x => x.Id == submissionId);
            if (s == null) return NotFound();
            return Ok(s);
        }

        [HttpPost("assignments/{assignmentId}/submissions/{submissionId}/grade")]
        public async Task<IActionResult> GradeSubmission(Guid assignmentId, Guid submissionId, [FromBody] GradeSubmissionDto dto)
        {
            if (dto.Grade < 0 || dto.Grade > 100) return BadRequest(new { error = "grade must be between 0 and 100" });
            var s = await _service.GradeSubmissionAsync(assignmentId, submissionId, dto.Grade, dto.Feedback);
            if (s == null) return NotFound();
            return Ok(s);
        }
    }
}
