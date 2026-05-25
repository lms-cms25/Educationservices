using System;
using System.Collections.Generic;

namespace Educationservices.Models
{
    public class Submission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssignmentId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public List<string> Links { get; set; } = new();
        public string Status { get; set; } = "Submitted";
        public double? Grade { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
