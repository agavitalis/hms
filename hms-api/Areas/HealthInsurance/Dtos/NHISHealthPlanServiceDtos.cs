using HMS.Models;
using System;


namespace HMS.Areas.HealthInsurance.Dtos
{
    public class NHISHealthPlanServiceDtoForCreate
    {
        public string ServiceId { get; set; }
        public string NHISHealthPlanId { get; set; }
    }

    public class NHISHealthPlanServiceDtoForUpdate
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public string NHISHealthPlanId { get; set; }
    }

    public class NHISHealthPlanServiceDtoForView
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public string NHISHealthPlanId { get; set; }
        public virtual NHISHealthPlan NHISHealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class NHISHealthPlanServiceDtoForDelete
    {
        public string Id { get; set; }
    }
}
