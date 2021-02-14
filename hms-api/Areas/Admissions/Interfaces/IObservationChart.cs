using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Interfaces
{
    public interface IObservationChart
    {
        Task<IEnumerable<ObservationChart>> GetAdmissionObservationChart(string AdmissionId);
        Task<bool> UpdateObservationChart(ObservationChartDtoForUpdate patientVitals);
    }
}
