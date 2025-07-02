namespace Pet.Core.Domain.Entities
{
    public class PetMedicalRecord
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string RecordType { get; set; }
        public DateTime RecordDate { get; set; }
        public string Details { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
