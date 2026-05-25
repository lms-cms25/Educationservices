namespace Educationservices.Models
{
    public class SchoolClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public string MentorName { get; set; } = string.Empty;
        public Guid ProgramId { get; set; }
    }
}
