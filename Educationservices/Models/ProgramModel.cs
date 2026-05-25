namespace Educationservices.Models
{
    public class ProgramModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
    }
}
