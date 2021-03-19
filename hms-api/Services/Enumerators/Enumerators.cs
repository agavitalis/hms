using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Enumerators
{
    public enum Roles
    {
        Accountant,
        Admin,
        Doctor,
        HMOAdmin,
        LabAttendant,
        Nurse,
        Patient,
        Pharmacist,
        WardPersonnel
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum BloodGenotype
    {
        AA,
        AS,
        SS,
        AC,
    }

    public enum BloodGroup
    {
        [Description("A+")]
        APositive,

        [Description("A-")]
        ANegative,

        [Description("B+")]
        BPositive,

        [Description("B-")]
        BNegative,

        [Description("O+")]
        OPositive,

        [Description("O-")]
        ONegative,

        [Description("AB+")]
        ABPositive,

        [Description("AB-")]
        ABNegative

    }

}
