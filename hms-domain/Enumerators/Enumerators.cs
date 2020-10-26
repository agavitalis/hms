using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Enumerators
{
    public enum Roles
    {
        Admin,
        Patient,
        Doctor,
        Accountant,
        Pharmacy,
        Lab,
        HMO,
    }

    public enum Gender
    {
        Male,
        Female,
        Other
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
