using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class ObservationChartRepository : IObservationChart
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ObservationChartRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<ObservationChart>> GetAdmissionObservationChart(string AdmissionId) => await _applicationDbContext.ObservationCharts.Where(o => o.AdmissionId == AdmissionId).ToListAsync();
     
        public async Task<bool> UpdateObservationChart(ObservationChartDtoForUpdate ObservationChart)
        {
            var observationChart = await _applicationDbContext.ObservationCharts.FirstOrDefaultAsync(o => o.AdmissionId == ObservationChart.AdmissionId && o.Date.Hour == DateTime.Now.Hour);

            // Validate vitals are not null---no vitals this hour
            if (observationChart == null)
            {
                var patientObservationChart = new ObservationChart()
                {
                    BloodPressure = ObservationChart.BloodPressure,
                    Pulse = ObservationChart.Pulse,
                    Respiration = ObservationChart.Respiration,
                    SPO2 = ObservationChart.SPO2,
                    Temperature = ObservationChart.Temperature,
                    Remarks = ObservationChart.Remarks,
                    AdmissionId = ObservationChart.AdmissionId,
                    InitiatorId = ObservationChart.InitiatorId
                };
                _applicationDbContext.ObservationCharts.Add(patientObservationChart);
                await _applicationDbContext.SaveChangesAsync();
            }
            else
            {
                observationChart.BloodPressure = ObservationChart.BloodPressure;
                observationChart.Pulse = ObservationChart.Pulse;
                observationChart.Respiration = ObservationChart.Respiration;
                observationChart.SPO2 = ObservationChart.SPO2;
                observationChart.Temperature = ObservationChart.Temperature;
                observationChart.Remarks = ObservationChart.Remarks;

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }

            return true;
        }
    }
}
