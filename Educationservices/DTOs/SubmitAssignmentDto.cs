using System.Collections.Generic;

namespace Educationservices.DTOs
{
    public class SubmitAssignmentDto
    {
        public string StudentId { get; set; } = string.Empty;
        public List<string> Links { get; set; } = new();
    }
}
