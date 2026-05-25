using System;

namespace Educationservices.DTOs
{
    public class CreateAssignmentDto
    {
        public string Title { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public DateTime OpeningDate { get; set; }
        public DateTime Deadline { get; set; }
    }
}
