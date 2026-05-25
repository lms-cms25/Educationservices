using System;

namespace Educationservices.Models
{
    public class Degree
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string StudentId { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        // Swedish school grading system: A (highest) - F (fail).
        public string Grade { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
