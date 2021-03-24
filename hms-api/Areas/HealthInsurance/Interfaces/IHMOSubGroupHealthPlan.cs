using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOSubUserGroupHealthPlan
    {
        Task<bool> CreateHMOSubUserGroupHealthPlan(HMOSubUserGroupHealthPlan hMOSubUserGroupHealthplan);
        Task<bool> DeleteHMOSubUserGroupHealthPlan(HMOSubUserGroupHealthPlan hMOSubUserGroupHealthplan);
    }
}
