using System.ComponentModel;

namespace TRT.Domain.Enums
{
    public enum Role
    {
        [Description("Back Office")]
        BackOffice = 1,

        [Description("Travel Agent")]
        TravelAgent = 2,
    }
}
