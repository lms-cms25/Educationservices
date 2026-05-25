using Educationservices.Data;
using Educationservices.DTOs;
using Educationservices.Models;
using Microsoft.EntityFrameworkCore;

namespace Educationservices.Services
{
    public class DegreeService
    {
        // Allowed Swedish school grades. A is best, F is fail.
        public static readonly string[] AllowedGrades = new[] { "A", "B", "C", "D", "E", "F" };

        private readonly AppDataContext _db;

        public DegreeService(AppDataContext db)
        {
            _db = db;
        }

        public async Task<List<Degree>> GetDegreesForCourseAsync(Guid courseId)
        {
            return await _db.Degrees.Where(d => d.CourseId == courseId).ToListAsync();
        }

        public async Task<List<Degree>> GetDegreesForStudentAsync(string studentId)
        {
            return await _db.Degrees.Where(d => d.StudentId == studentId).ToListAsync();
        }

        // Assigning a degree replaces any existing one for the same student in the same course,
        // so an instructor can correct a mistake without creating duplicates.
        public async Task<Degree> AssignDegreeAsync(Guid courseId, AssignDegreeDto dto)
        {
            var existing = await _db.Degrees.FirstOrDefaultAsync(
                d => d.CourseId == courseId && d.StudentId == dto.StudentId);

            if (existing != null)
            {
                existing.Grade = dto.Grade;
                existing.Comment = dto.Comment;
                existing.CreatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return existing;
            }

            var degree = new Degree
            {
                CourseId = courseId,
                StudentId = dto.StudentId,
                Grade = dto.Grade,
                Comment = dto.Comment,
            };
            _db.Degrees.Add(degree);
            await _db.SaveChangesAsync();
            return degree;
        }
    }
}
