namespace Pet.Core.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string ReportType { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? AppUserId { get; set; }
        public int? PetId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public virtual Pet? Pet { get; set; }
    }
}
