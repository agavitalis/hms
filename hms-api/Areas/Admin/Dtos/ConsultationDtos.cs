using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class ConsultationDtoForUpdate
    {
        public string Id { get; set; }
        public DateTime DateOfConsultation { get; set; }
        public Boolean IsCanceled { get; set; }
        public Boolean IsCompleted { get; set; }
        public Boolean IsExpired { get; set; }
        public string ConsultationTitle { get; set; }
        public string ReasonForConsultation { get; set; }
        public Boolean IsNewPatient { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
    }
}
