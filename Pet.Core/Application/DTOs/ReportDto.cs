namespace Pet.Core.Application.DTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string ReportType { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? AppUserId { get; set; }
        public int? PetId { get; set; }
    }
}
