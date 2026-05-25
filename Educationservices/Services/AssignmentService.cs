using Educationservices.Data;
using Educationservices.DTOs;
using Educationservices.Models;
using Microsoft.EntityFrameworkCore;

namespace Educationservices.Services
{
    public class AssignmentService
    {
        private readonly AppDataContext _db;

        public AssignmentService(AppDataContext db)
        {
            _db = db;
        }

        public async Task<List<ProgramModel>> GetCoursesAsync()
        {
            return await _db.Programs.ToListAsync();
        }

        public async Task<List<Assignment>> GetAssignmentsForCourseAsync(Guid courseId)
        {
            return await _db.Assignments.Where(a => a.CourseId == courseId).ToListAsync();
        }

        public async Task<Assignment?> GetAssignmentAsync(Guid courseId, Guid assignmentId)
        {
            return await _db.Assignments.FirstOrDefaultAsync(a => a.CourseId == courseId && a.Id == assignmentId);
        }

        public async Task<Assignment> CreateAssignmentAsync(Guid courseId, CreateAssignmentDto dto)
        {
            var assignment = new Assignment
            {
                CourseId = courseId,
                Title = dto.Title,
                Instructions = dto.Instructions,
                OpeningDate = dto.OpeningDate,
                Deadline = dto.Deadline,
            };
            _db.Assignments.Add(assignment);
            await _db.SaveChangesAsync();
            return assignment;
        }

        public async Task<Submission> SubmitAssignmentAsync(Guid assignmentId, SubmitAssignmentDto dto)
        {
            var submission = new Submission
            {
                AssignmentId = assignmentId,
                StudentId = dto.StudentId,
                Links = dto.Links,
            };
            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();
            return submission;
        }

        public async Task<List<Submission>> GetSubmissionsAsync(Guid assignmentId)
        {
            return await _db.Submissions.Where(s => s.AssignmentId == assignmentId).ToListAsync();
        }

        public async Task<Submission?> GetStudentSubmissionAsync(Guid assignmentId, string studentId)
        {
            return await _db.Submissions.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
        }

        public async Task<Submission?> GradeSubmissionAsync(Guid assignmentId, Guid submissionId, double grade, string? feedback)
        {
            var s = await _db.Submissions.FirstOrDefaultAsync(x => x.Id == submissionId && x.AssignmentId == assignmentId);
            if (s == null) return null;
            s.Grade = grade;
            s.Feedback = feedback;
            s.Status = "Graded";
            s.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return s;
        }
    }
}
