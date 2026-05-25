namespace Educationservices.DTOs
{
    public class AssignDegreeDto
    {
        public string StudentId { get; set; } = string.Empty;
        // Swedish letter grade: A, B, C, D, E or F.
        public string Grade { get; set; } = string.Empty;
        public string? Comment { get; set; }
    }
}
