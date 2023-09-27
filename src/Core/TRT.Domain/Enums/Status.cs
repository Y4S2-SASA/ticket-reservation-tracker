using System.ComponentModel;

namespace TRT.Domain.Enums
{
    public enum Status
    {
        [Description("Pending")]
        Pending =1,

        [Description("Activated")]
        Activated =2,

        [Description("Deactivated")]
        Deactivated =3,

        [Description("Deleted")]
        Deleted =4
    }
}
