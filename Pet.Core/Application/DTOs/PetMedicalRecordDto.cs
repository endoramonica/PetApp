namespace Pet.Core.Application.DTOs
{
    public class PetMedicalRecordDto
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string RecordType { get; set; }
        public DateTime RecordDate { get; set; }
        public string Details { get; set; }
    }
}
