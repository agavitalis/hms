using System;

namespace HMS.Models
{
    public class AdmissionNote
    {
        public AdmissionNote()
        {
            Id = Guid.NewGuid().ToString();
            DateGenerated = DateTime.Now;
        }

        public string Id { get; set; }
        public string Note { get; set; }
        public string AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public DateTime DateGenerated { get; set; }
    }
}
